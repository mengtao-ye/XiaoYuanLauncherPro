using System.IO;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class LoadHotVersionProcess : BaseProcess
    {
        private string mHotVersion;//热更版本
        private LoadMainFileChildController mController;
        public LoadHotVersionProcess(LoadMainFileChildController controller)
        {
            mController = controller;
        }
        public override void Enter()
        {
            LoadAppHotVersionFile();
        }
        /// <summary>
        /// 加载原始数据版本文件
        /// </summary>
        private void LoadAppHotVersionFile()
        {
            string ossVersionPath = OssData.GetOssHotVersionPath();
            AliyunOSSTools.Instance.LoadOssString(ossVersionPath, LoadProcess, LoadHotVersionSuccess, LoadVersionFail);
        }

        /// <summary>
        /// 版本文件下载失败回调
        /// </summary>
        /// <param name="str"></param>
        private void LoadVersionFail(string str)
        {
            LogHelper.LogError("热更版本数据加载失败,Error:" + str);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>((ui) => {
                ui.ShowContent("热更版本数据下载失败，是否重试？", "资源下载失败", "退出", "重试", LoadVersionSureCallBack, LoadCancelCallBack);
            });
        }
        /// <summary>
        /// 加载版本失败后确认按钮回调
        /// </summary>
        private void LoadVersionSureCallBack()
        {
            AppTools.HideTipsUI<CommonTwoSelectTipUI>();
            LoadAppHotVersionFile();
        }

        private void LoadHotVersionSuccess(string version)
        {
            if (version.IsNullOrEmpty() || version == "0")
            {
                //没有热更资源 
                LogHelper.Log("无热更资源");
                (processManager as ProcessManager<LoadOssCondition>).condition.hotVersion = "0";
                DoNext();
                return;
            }
            string versionPath = OssData.GetLocalHotDir(ABTag.Main) + "/Version.txt";
            if (!File.Exists(versionPath))
            {
                //本地还没这个版本文件，说明应该是第一次进去的时候
                FileTools.Write(versionPath, version);//写入版本
                mHotVersion = version;
            }
            else
            {
                mHotVersion = File.ReadAllText(versionPath);
                int localVersionCode = AppTools.GetVersionCode(mHotVersion);
                int serverVersionCode = AppTools.GetVersionCode(version);
                if (serverVersionCode != localVersionCode)
                {
                    //如果服务器端的版本跟本地不一致的话，就下载最新的资源
                    DirectoryTools.ClearDir(OssData.GetLocalHotDir(ABTag.Main));
                    FileTools.Write(versionPath, version);//写入版本
                    mHotVersion = version;
                }
            }
           (processManager as ProcessManager<LoadOssCondition>).condition.hotVersion = mHotVersion;
            DoNext();
        }
        private void LoadProcess(float process)
        {

        }
        /// <summary>
        /// 版本文件下载失败回调
        /// </summary>
        private void LoadCancelCallBack()
        {
            AppTools.QuitApp();
        }
    }
}
