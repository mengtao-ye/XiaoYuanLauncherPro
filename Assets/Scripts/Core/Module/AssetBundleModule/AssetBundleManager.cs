using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YFramework;
using static YFramework.Core.Utility.Utility;
using static YFramework.Utility;

namespace Game
{
    public  class AssetBundleManager
    {
        public string name { get; private set; }
        /// <summary>
        /// 所有加载的资源
        /// </summary>
        private  Dictionary<ulong, ResourceItem> mABResourceDict = new Dictionary<ulong, ResourceItem>();
        /// <summary>
        /// 加载的Asset Bundle包
        /// </summary>
        private  Dictionary<ulong, AssetBundleItem> mAssetBundleItemDict = new Dictionary<ulong, AssetBundleItem>();
        public  Dictionary<ulong, AssetBundleItem> assetBundleItemDict { get { return mAssetBundleItemDict; } }

        public AssetBundleManager(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// 初始化资源
        /// </summary>
        public  void Init(byte[] data)
        {
            //AssetBundlePathData.ASSETBUNDLE_PATH + "/config"
            AssetBundle ab = AssetBundle.LoadFromMemory(data);
            TextAsset textAsset =  ab.LoadAsset<TextAsset>("BinaryABConfig");
            AssetBundleConfigData config = ConverterDataTools.ToObject<AssetBundleConfigData>(textAsset.bytes);  //BinaryMapper.ToObject<AssetBundleConfigData>(textAsset.bytes);
            if (config == null)
            {
                Debug.LogError("配置表读取错误！");
                return;
            }
            for (int i = 0; i < config.ABList.Count; i++)
            {
                ABBase abBase = config.ABList[i];
                ResourceItem resourceItem = new ResourceItem(name);

                resourceItem.ABName = abBase.ABName;
                resourceItem.AssetName = abBase.AssetName;
                resourceItem.CRC = abBase.CRC;
                resourceItem.Dependence = abBase.Dependence;
                mABResourceDict.Add(resourceItem.CRC, resourceItem);
            }
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public  ResourceItem LoadResourceItem(ulong crc)
        {
            ResourceItem resource = mABResourceDict.TryGet(crc);
            if (resource == null)
            {
                Debug.LogError(string.Format("没有对应的Asset Bundle资源！"));
                return null;
            }
            resource.CRC = crc;
            resource.AssetBundle = LoadAssetBundle(resource.ABName, name);
            if (!resource.Dependence.IsNullOrEmpty()) {
                for (int i = 0; i < resource.Dependence.Count; i++)
                {
                    LoadAssetBundle(resource.Dependence[i], name);
                }
            }
            return resource;
        }
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="item"></param>
        public  void ReleaseResource(ResourceItem item)
        {
            if (item == null) return;
            if (!item.Dependence.IsNullOrEmpty()) {
                for (int i = 0; i < item.Dependence.Count; i++)
                {
                    ReleaseResourceItem(item.Dependence[i]);
                }
            }
            ReleaseResourceItem(item.ABName);
        }
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetBundleName"></param>
        private  void ReleaseResourceItem(string assetBundleName) {
            if (string.IsNullOrEmpty(assetBundleName)) return;
            ulong crc = CRC32Tool.GetCRC32(assetBundleName);
            AssetBundleItem item = mAssetBundleItemDict.TryGet(crc);
            if (item == null) {
                return;
            }
            item.count--;
        }
        /// <summary>
        /// 加载AssetBundle包
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        private  AssetBundle LoadAssetBundle(string assetBundleName,string packageName) 
        {
            string path = AssetBundleModule.GetAssetPath(packageName, assetBundleName);
            if (!File.Exists(path)) {
                Debug.LogError(string.Format("地址{0}不存在",path));
                return null;
            }
            ulong crc = CRC32Tool.GetCRC32(assetBundleName);
            AssetBundleItem assetBundleItem = mAssetBundleItemDict.TryGet(crc);
            if (assetBundleItem == null)
            {
                assetBundleItem = ClassPool<AssetBundleItem>.Pop();
                assetBundleItem.SetManagerName(packageName);
                byte[] bytes = File.ReadAllBytes(path);
                EncryptionTools.Decryption(bytes);
                AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
                assetBundleItem.assetBundle = assetBundle;
                assetBundleItem.CRC = crc;
                assetBundleItem.count++;
                mAssetBundleItemDict.Add(crc, assetBundleItem);
            }
            else 
            {
                assetBundleItem.count++;
            }
            return assetBundleItem.assetBundle;
        }
    }
}