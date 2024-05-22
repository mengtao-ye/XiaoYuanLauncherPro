using System.IO;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class LoadConfigProcess : BaseProcess
    {
        private LoadMainFileChildController mController;
        private ProcessManager<LoadOssCondition> mManager;

        public LoadConfigProcess(LoadMainFileChildController controller)
        {
            mController = controller;
        }

        public override void Init()
        {
            mManager = (processManager as ProcessManager<LoadOssCondition>);
        }

        public override void Enter()
        {
            StartDownLoadOriginalFile();
        }

        /// <summary>
        /// 开始下载主资源文件
        /// </summary>
        private void StartDownLoadOriginalFile()
        {
            string version = mManager.condition.version;    
            mController.initPanel.SetContent("加载主数据配置表");
            string localABFileConfigPath = OssData. GetLocalOriginalDir(ABTag.Main) + "/ABFileConfig.bytes";
            if (File.Exists(localABFileConfigPath))
            {
                LoadConfig(File.ReadAllBytes(localABFileConfigPath));
            }
            else
            {
                string configPath = OssData.GetOssOriginalFilePath(version, "ABFileConfig.bytes");
               HttpTools.GetBytes(configPath, LoadProcess, DownLoadConfigSuccess, LoadConfigFailCallback);
            }
        }
        /// <summary>
        /// 版本文件下载失败回调
        /// </summary>
        /// <param name="str"></param>
        private void LoadConfigFailCallback(string str)
        {
            LogHelper.LogError("配置数据加载失败,Error:" + str);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>((ui) => {
                ui.ShowContent("配置文件数据加载失败，是否重试？", "配置下载失败", "退出", "重试", StartDownLoadOriginalFile, LoadCancelCallBack);
            });
        }

        private void DownLoadConfigSuccess(byte[] bytes)
        {
            string localABFileConfigPath = OssData.GetLocalOriginalDir(ABTag.Main) + "/ABFileConfig.bytes";
            File.WriteAllBytes(localABFileConfigPath,bytes);
            LoadConfig(bytes);
        }

        private void LoadConfig(byte[] bytes)
        {
            AssetBundleFileData data = AliyunOSSTools.Instance.LoadFileConfig(bytes);
            if (data == null)
            {
                LoadConfigFailCallback("解析主资源配置文件失败");
                return;
            }
            mManager.condition.config = data;
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
