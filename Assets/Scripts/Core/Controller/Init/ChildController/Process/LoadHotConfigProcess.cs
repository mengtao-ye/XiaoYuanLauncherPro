using System.IO;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 加载热更数据
    /// </summary>
    public class LoadHotConfigProcess : BaseProcess
    {
        private ProcessManager<LoadOssCondition> mManager;
        private LoadMainFileChildController mController;
        public LoadHotConfigProcess(LoadMainFileChildController controller)
        {
         
            mController = controller;
        }
        public override void Init()
        {
            mManager = processManager as ProcessManager<LoadOssCondition>;
        }
        public override void Enter()
        {
            string version = mManager.condition.hotVersion;
            if (version == "0")//无热更版本
            {
                DoNext();
                return;
            }
            StartDownLoadConfig();
        }
        /// <summary>
        /// 开始下载主资源文件
        /// </summary>
        private void StartDownLoadConfig()
        {
            mController.initPanel.SetContent("加载热更数据配置表");
            string version = (processManager as ProcessManager<LoadOssCondition>).condition.hotVersion;
            string configPath = OssData.GetOssHotFilePath(version, "HotABFileConfig.bytes");
            AliyunOSSTools.Instance.LoadOssBytes(configPath, LoadProcess, LoadConfigSuccessCallback, LoadConfigFailCallback);
        }
        /// <summary>
        /// 版本文件下载失败回调
        /// </summary>
        /// <param name="str"></param>
        private void LoadConfigFailCallback(string str)
        {
            LogHelper.LogError("热更配置数据加载失败,Error:" + str);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>((ui) => {
                ui.ShowContent("热更配置文件数据加载失败，是否重试？", "配置下载失败", "退出", "重试", StartDownLoadConfig, LoadCancelCallBack);
            });
        }

        private void LoadConfigSuccessCallback(byte[] bytes)
        {
            AssetBundleFileData data = AliyunOSSTools.Instance.LoadFileConfig(bytes);
            if (data == null)
            {
                LoadConfigFailCallback("解析热更资源配置文件失败");
                return;
            }
            mManager.condition.hotConfig = data;
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
