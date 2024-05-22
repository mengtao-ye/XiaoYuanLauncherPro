using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class AliyunOSSTools : Singleton<AliyunOSSTools>
    {
        /// <summary>
        /// 加载文件配置表
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        public AssetBundleFileData LoadLocalFileConfig(string resPath)
        {
            if (!File.Exists(resPath))
            {
                Debug.LogError("未找到ABFile配置表");
                return null;
            }
            byte[] data = File.ReadAllBytes(resPath);
            return LoadFileConfig(data);
        }
        /// <summary>
        /// 加载文件配置表(不解密)
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        public AssetBundleFileData LoadFileConfig(byte[] datas)
        {
            if (datas.IsNullOrEmpty())
            {
                LogHelper.LogError("LoadFileConfig data is empty or null");
                return null;
            }
            AssetBundleFileData mAssetBundleFileData = ConverterDataTools.ToObject<AssetBundleFileData>(datas);
            if (mAssetBundleFileData == null)
            {
                Debug.LogError("ABFile配置表数据错误");
                return null;
            }
            return mAssetBundleFileData;
        }
    
    }
}
