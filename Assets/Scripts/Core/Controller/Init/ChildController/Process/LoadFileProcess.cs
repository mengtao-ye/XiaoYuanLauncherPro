using System.IO;
using YFramework;

namespace Game
{
    public class LoadFileProcess : BaseProcess
    {
        private LoadMainFileChildController mController;
        private ProcessManager<LoadOssCondition> mManager;
        private int mCurIndex;//当前下载到哪个资源下标了
        private long mCurDownLoadSize;//当前下载了多少数据
        public LoadFileProcess(LoadMainFileChildController controller)
        {
            mController = controller;
        }

        public override void Init()
        {
            mManager = (processManager as ProcessManager<LoadOssCondition>);
        }

        public override void Enter()
        {
            LoadOriginalFile();
        }

        private void LoadOriginalFile()
        {
            if (mCurIndex == mManager.condition.config.fileNames.Count)
            {
                //资源全部下载完成
                LogHelper.Log("主资源下载完成");
                DoNext();
                mController.ABAssetLoadSuccess();
            }
            else
            {
                string fileName = mManager.condition.config.fileNames[mCurIndex];
                string localPath = OssData.GetLocalOriginalDir(ABTag.Main) + "/" + fileName;
                if (mManager.condition.hotConfig!=null&&!mManager.condition.hotConfig.fileNames .IsNullOrEmpty() && mManager.condition.hotConfig.fileNames.Contains(fileName))
                {
                    //如果热更文件里包含了主资源文件，说明这个文件是热更文件
                    LoadOriginalSuccessCallback(0);
                }
                else
                {
                    if (!File.Exists(localPath))
                    {
                        //如果版本没有这个文件的话就下载
                        mController.initPanel.SetContent("加载主资源数据：" + fileName);
                        string ossPath = OssData.GetOssOriginalFilePath(mManager.condition.version, fileName);
                        YFramework.Utility.HttpTools.GetBytesToLocal  (ossPath, OssData.GetLocalOriginalDir(ABTag.Main) + "/" + fileName,  LoadOriginalFileFailCallback, LoadOriginalSuccessCallback);
                    }
                    else
                    {
                        if (mManager.condition.hotVersion != "0") //有热更文件的情况下
                        {
                            //热更文件不包含改文件但是本地还有这个文件的话说明该文件热更的时候去掉了
                            File.Delete(localPath);
                        }
                        //已经有这个文件的话
                        int size =  (int)new System.IO.FileInfo(localPath).Length;
                        LoadOriginalSuccessCallback(size);
                    }
                }
            }
        }

        /// <summary>
        /// 加载主资源失败回调
        /// </summary>
        /// <param name="str"></param>
        private void LoadOriginalFileFailCallback(string str)
        {
            LogHelper.LogError("主资源加载失败,Error:" + str);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>((ui => {
                ui.ShowContent("主资源数据加载失败，是否重试？", "资源下载失败", "退出", "重试", LoadOriginalFile, LoadCancelCallBack);
            }));
        }

        /// <summary>
        /// 加载主资源成功
        /// </summary>
        /// <param name="size"></param>
        private void LoadOriginalSuccessCallback(int size)
        {
            mCurIndex++;
            LoadOriginalFile();
            mCurDownLoadSize += size;
            mController.initPanel.SetProcess(mCurDownLoadSize / mManager.condition.config.size);
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
