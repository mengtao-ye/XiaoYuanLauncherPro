
using System.IO;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 加载热更介绍内容流程
    /// </summary>
    public class LoadHotIntroduceProcess : BaseProcess
    {
        private string mDefaultIntroduce = "资源更新";
        private LoadMainFileChildController mController;
        private string version;
        public LoadHotIntroduceProcess(LoadMainFileChildController controller)
        {
            mController = controller;
        }
        public override void Enter()
        {
            version = (processManager as ProcessManager<LoadOssCondition>).condition.hotVersion;
            if (version == "0")//无热更版本
            {
                DoNext();
                return;
            }
            LoadIntroduce();
        }

        private void LoadIntroduce()
        {
            string introduceOssPath = OssData.GetOssHotFilePath(version, "Introduce.txt");
            AliyunOSSTools.Instance.LoadOssString(introduceOssPath, LoadProcess, LoadIntroduceSuccess, LoadIntroduceFail);
        }

        private void LoadProcess(float process)
        {

        }
        private void LoadIntroduceFail(string content)
        {
            LogHelper.LogError("新资源介绍为空,Error:" + content);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>((ui) => {
               ui .ShowContent("新资源数据加载失败，是否重试？", "资源下载失败", "退出", "重试", RetryDownLoadIntroduce, LoadCancelCallBack);
            });
        }
        private void LoadIntroduceSuccess(string content)
        {
            if (content.IsNullOrEmpty())
            {
                LogHelper.LogError("服务器端新版本介绍为空");
                content = mDefaultIntroduce;
            }
            AppTools.ShowTipsUI<HotVersionTipUI>((ui) => { 
               ui.ShowContent(content, DownLoadHotVersionCallBack);
            });
        }
        /// <summary>
        /// 开始下载热更版本资源
        /// </summary>
        private void DownLoadHotVersionCallBack()
        {
            DoNext();
        }
        /// <summary>
        /// 重试下载新版本介绍
        /// </summary>
        private void RetryDownLoadIntroduce()
        {
            string introduceOssPath = OssData.GetOssHotFilePath(version, "Introduce.txt");
            AliyunOSSTools.Instance.LoadOssString(introduceOssPath, LoadProcess, LoadIntroduceSuccess, LoadIntroduceFail);
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
