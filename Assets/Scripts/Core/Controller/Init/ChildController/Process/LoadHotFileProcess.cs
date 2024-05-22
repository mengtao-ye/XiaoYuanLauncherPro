using System.IO;
using YFramework;

namespace Game
{
    public class LoadHotFileProcess : BaseProcess
    {
        private LoadMainFileChildController mController;
        private int mCurIndex;//当前下载到哪个资源下标了
        private long mCurDownLoadSize;//当前下载了多少数据
        private ProcessManager<LoadOssCondition> mManager;
        public LoadHotFileProcess(LoadMainFileChildController controller)
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
            LoadHotFile();
        }

        private void LoadHotFile()
        {
            if (mCurIndex == mManager.condition.hotConfig.fileNames.Count)
            {
                //资源全部下载完成
                LogHelper.Log("热更资源下载完成");
                DoNext();
            }
            else
            {
                string fileName = mManager.condition.hotConfig.fileNames[mCurIndex];
                string localPath = OssData.GetLocalHotDir(ABTag.Main) + "/" + fileName;
                if (!File.Exists(localPath))
                {
                    //如果版本没有这个文件的话就下载
                    mController.initPanel.SetContent("加载热更资源数据：" + fileName);
                    string ossPath = OssData.GetOssHotFilePath(mManager.condition.hotVersion, fileName);
                    YFramework.Utility.HttpTools .GetBytesToLocal(ossPath, OssData.GetLocalHotDir(ABTag.Main) + "/" + fileName, LoadHotFileFailCallback,LoadHotSuccessCallback);
                }
                else
                {
                    //已经有这个文件的话
                    int size =(int) new System.IO.FileInfo(localPath).Length;
                    LoadHotSuccessCallback(size);
                }
            }
        }

        /// <summary>
        /// 加载热更资源失败回调
        /// </summary>
        /// <param name="str"></param>
        private void LoadHotFileFailCallback(string str)
        {
            LogHelper.LogError("热更资源加载失败,Error:" + str);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>((ui) => {
                ui.ShowContent("热更资源数据加载失败，是否重试？", "资源下载失败", "退出", "重试", LoadHotFile, LoadCancelCallBack);
            });
        }

        /// <summary>
        /// 加载热更资源成功
        /// </summary>
        /// <param name="size"></param>
        private void LoadHotSuccessCallback(int size)
        {
            mCurIndex++;
            LoadHotFile();
            mCurDownLoadSize += size;
            mController.initPanel.SetProcess(mCurDownLoadSize / mManager.condition.hotConfig.size);
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
