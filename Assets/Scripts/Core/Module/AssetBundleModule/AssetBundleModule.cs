using System.Collections.Generic;
using System.IO;
using YFramework;

namespace Game
{
    public class AssetBundlePair 
    {
        public AssetBundleManager assetBundleManager;
        public LoadDll dll;
    }

    public static class AssetBundleModule
    {
        private static Dictionary<string, AssetBundlePair> mDict = new Dictionary<string, AssetBundlePair>();
        /// <summary>
        ///主资源
        /// </summary>
        public static AssetBundleManager Main
        {
            get
            {
                return Get(AssetBundleConstName.MAIN_NAME);
            }
        }
        public static void Add(string packageName)
        {
            if (mDict.ContainsKey(packageName)) return;
            AssetBundlePair pair = new AssetBundlePair();
            LoadDll dll = new LoadDll();
            string hotbasePath = GetAssetPath(packageName, "hotbase");
            byte[] dllData = File.ReadAllBytes(hotbasePath);
            EncryptionTools.Decryption(dllData);
            dll.Init("HotBase.dll", dllData );
            pair.dll = dll;
            AssetBundleManager assetBundleManager = new AssetBundleManager(packageName);
            string configPath = GetAssetPath(packageName,"config");
            byte[] abData = File.ReadAllBytes(configPath);
            EncryptionTools.Decryption(abData);
            assetBundleManager.Init(abData);
            pair.assetBundleManager = assetBundleManager;
            mDict.Add(packageName, pair);
        }
        /// <summary>
        /// 获取资源管理器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AssetBundleManager Get(string name)
        {
            if (mDict.ContainsKey(name)) return mDict[name].assetBundleManager;
            return null;
        }
        /// <summary>
        /// 获取资源地址
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetAssetPath(string packageName,string assetName) 
        {
            string path = OssData.GetLocalHotDir(packageName) + "/"+ assetName;
            if (!File.Exists(path))
            {
                path = OssData.GetLocalOriginalDir(packageName) +   "/" + assetName;
                if (!File.Exists(path))
                {
                    LogHelper.LogError(assetName +"资源不存在");
                    return null;
                }
            }
            return path;
        }
       
    }
}
