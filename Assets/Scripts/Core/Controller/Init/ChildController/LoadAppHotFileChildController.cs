using System.IO;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 加载app原始数据
    /// </summary>
    public class LoadAppHotFileChildController : BaseCustomChildController
    {
        private InitPanel mInitPanel;
        private string mHotVersion;//热更版本
        private AssetBundleFileData mConfigData;//配置数据
        private int mCurIndex;//当前下载到哪个资源下标了
        private long mCurDownLoadSize;//当前下载了多少数据
        public LoadAppHotFileChildController(IController controller) : base(controller)
        {
        }
        public override void Awake()
        {
            mInitPanel = controller.scene.canvas.FindPanel<InitPanel>();
            //检查有没有热更版本
            LoadAppHotVersionFile();
        }
        /// <summary>
        /// 加载原始数据版本文件
        /// </summary>
        private void LoadAppHotVersionFile()
        {
            string ossVersionPath = OssData.GetHotVersionPath();
            AliyunOSSTools.Instance.LoadOssString(ossVersionPath, LoadProcess, LoadSuccess, LoadVersionFail);
        }

        private void LoadProcess(float process)
        {

        }

        private void LoadSuccess(string version)
        {
            if (version.IsNullOrEmpty() || version == "0")
            {
                //没有热更资源
                Log.Loger("无热更资源");
                controller.RemoveChildController(this);
                AppTools.LoadScene(SceneID.LoginScene);
                return;
            }
            else 
            {

            }
        }

        private void CheckHotVersion(string version)
        {

            string versionPath = OssData.GetHotLocalDir() + "/Version.txt";
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
                if (serverVersionCode > localVersionCode)
                {
                    //如果服务器端的版本更加新的话，就下载最新的资源
                    DirectoryTools.ClearDir(OssData.GetHotLocalDir());
                    FileTools.Write(versionPath, version);//写入版本
                    mHotVersion = version;
                }
            }
            StartDownLoadHotFile();
        }

        /// <summary>
        /// 开始下载主资源文件
        /// </summary>
        private void StartDownLoadHotFile()
        {
            mInitPanel.SetContent("加载热更数据配置表");
            string configPath = OssData.GetOriginalFilePath(mHotVersion, "ABFileConfig.bytes");
            AliyunOSSTools.Instance.LoadOssBytes(configPath, LoadProcess, LoadConfigSuccessCallback, LoadConfigFailCallback);
        }
        private void LoadConfigSuccessCallback(byte[] bytes)
        {
            AssetBundleFileData data = AliyunOSSTools.Instance.LoadFileConfig(bytes);
            if(data == null)
            {
               LoadConfigFailCallback("解析主资源配置文件失败");
               return;
            }
            mConfigData = data;
            LoadOriginalFile();
        }
        private void LoadOriginalFile()
        {
            if (mCurIndex == mConfigData.fileNames.Count)
            {
                //资源全部下载完成
                Log.Loger("主资源下载完成");

            }
            else 
            {
                string fileName = mConfigData.fileNames[mCurIndex];
                string localPath = OssData.GetOriginalLocalDir() + "/" + fileName;
                if (!File.Exists(localPath))
                {
                    //如果版本没有这个文件的话就下载
                    mInitPanel.SetContent("加载主资源数据：" + fileName);
                    string ossPath = OssData.GetOriginalFilePath(mHotVersion, fileName);
                    AliyunOSSTools.Instance.LoadOssFileToLocal(ossPath, OssData.GetOriginalLocalDir() + "/" + fileName, LoadProcess, LoadOriginalSuccessCallback, LoadOriginalFileFailCallback);
                }
                else 
                {
                    //已经有这个文件的话
                    long size = new System.IO.FileInfo(localPath).Length;
                    LoadOriginalSuccessCallback(size);
                }
            }
        }

        /// <summary>
        /// 加载主资源成功
        /// </summary>
        /// <param name="size"></param>
        private void LoadOriginalSuccessCallback(long size)
        {
            mCurIndex++;
            LoadOriginalFile();
            mCurDownLoadSize += size;
            mInitPanel.SetProcess(mCurDownLoadSize/ mConfigData.size);
        }

        /// <summary>
        /// 加载主资源失败回调
        /// </summary>
        /// <param name="str"></param>
        private void LoadOriginalFileFailCallback(string str)
        {
            Log.LogError("主资源加载失败,Error:" + str);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>().ShowContent("主资源数据加载失败，是否重试？", "资源下载失败", "退出", "重试", LoadOriginalFile, LoadCancelCallBack);
        }



        /// <summary>
        /// 版本文件下载失败回调
        /// </summary>
        /// <param name="str"></param>
        private void LoadConfigFailCallback(string str)
        {
            Log.LogError("配置数据加载失败,Error:"+str);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>().ShowContent("配置文件数据加载失败，是否重试？", "配置下载失败", "退出", "重试", StartDownLoadHotFile, LoadCancelCallBack);
        }


        /// <summary>
        /// 版本文件下载失败回调
        /// </summary>
        /// <param name="str"></param>
        private void LoadVersionFail(string str)
        {
            Log.LogError("热更版本数据加载失败,Error:"+str);
            AppTools.ShowTipsUI<CommonTwoSelectTipUI>().ShowContent("热更版本数据下载失败，是否重试？","资源下载失败","退出","重试", LoadVersionSureCallBack, LoadCancelCallBack);
        }
        /// <summary>
        /// 加载版本失败后确认按钮回调
        /// </summary>
        private void LoadVersionSureCallBack()
        {
            AppTools.HideTipsUI<CommonTwoSelectTipUI>();
            LoadAppHotVersionFile();
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
