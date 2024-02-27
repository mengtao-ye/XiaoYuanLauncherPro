using System.IO;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class LoadOriginalVersionProcess : BaseProcess
    {
        private string mOriginalVersion;//主数据版本
        private LoadMainFileChildController mController;
        public LoadOriginalVersionProcess(LoadMainFileChildController controller)
        {
            mController = controller;
        }
        public override void Enter()
        {
            LoadAppOriginalVersionFile();
        }
        /// <summary>
        /// 加载原始数据版本文件
        /// </summary>
        private void LoadAppOriginalVersionFile()
        {
            string ossVersionPath = OssData.GetOssOriginalVersionPath();
            mController.initPanel.SetContent("加载主数据版本");
            mController.initPanel.SetProcess(0);
            AliyunOSSTools.Instance.LoadOssString(ossVersionPath, LoadProcess, LoadSuccess, LoadVersionFail);
        }
        /// <summary>
        /// 版本文件下载失败回调
        /// </summary>
        /// <param name="str"></param>
        private void LoadVersionFail(string str)
        {
            LogHelper.LogError("版本数据加载失败,Error:" + str);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>((ui) => {
                ui.ShowContent("版本数据下载失败，是否重试？", "资源下载失败", "退出", "重试", LoadVersionSureCallBack, LoadCancelCallBack);
            });
        }
        /// <summary>
        /// 加载版本失败后确认按钮回调
        /// </summary>
        private void LoadVersionSureCallBack()
        {
            AppTools.HideTipsUI<CommonTwoSelectTipUI>();
            LoadAppOriginalVersionFile();
        }
        /// <summary>
        /// 版本文件下载失败回调
        /// </summary>
        private void LoadCancelCallBack()
        {
            AppTools.QuitApp();
        }
        private void LoadProcess(float process)
        {

        }
        private void LoadSuccess(string version)
        {
            if (version.IsNullOrEmpty())
            {
                LogHelper.LogError("版本数据加载失败");
                AppTools.ShowTipsUI<CommonTwoSelectTipUI>((ui => {
                    ui.ShowContent("版本数据下载失败，是否重试？", "资源下载失败", "退出", "重试", LoadVersionSureCallBack, LoadCancelCallBack);
                }));
                return;
            }
            string versionPath = OssData.GetLocalOriginalDir(ABTag.Main) + "/Version.txt";
            if (!File.Exists(versionPath))
            {
                //本地还没这个版本文件，说明应该是第一次进去的时候
                FileTools.Write(versionPath, version);//写入版本
                mOriginalVersion = version;
            }
            else
            {
                mOriginalVersion = File.ReadAllText(versionPath);
                int localVersionCode = AppTools.GetVersionCode(mOriginalVersion);
                int serverVersionCode = AppTools.GetVersionCode(version);
                if (serverVersionCode != localVersionCode)
                {
                    //如果服务器端的版本跟本地版本不一致的话，就下载最新的资源
                    DirectoryTools.ClearDir(OssData.GetLocalOriginalDir(ABTag.Main));
                    FileTools.Write(versionPath, version);//写入版本
                    mOriginalVersion = version;
                }
            }
            (processManager as ProcessManager<LoadOssCondition>).condition.version = mOriginalVersion;
            DoNext();
        }
    }
}
