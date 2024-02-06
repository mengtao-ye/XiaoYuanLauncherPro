using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

        public static void Add(string name, string assetDict)
        {
            if (mDict.ContainsKey(name)) return;
            AssetBundlePair pair = new AssetBundlePair();
            LoadDll dll = new LoadDll();
            byte[] dllData = File.ReadAllBytes(assetDict + "/hotbase");
            EncryptionTools.Decryption(dllData);
            dll.Init("HotBase.dll", dllData );
            pair.dll = dll;
            AssetBundleManager assetBundleManager = new AssetBundleManager(name);
            byte[] abData = File.ReadAllBytes(assetDict + "/config");
            EncryptionTools.Decryption(abData);
            assetBundleManager.Init(abData);
            pair.assetBundleManager = assetBundleManager;
            mDict.Add(name, pair);
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
       
    }
}
