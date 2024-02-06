using UnityEngine;
using YFramework;

namespace Game
{
    public static class OssData
    {
        /// <summary>
        /// 获取Oss数据主资源本地目录
        /// </summary>
        /// <returns></returns>
        public static string GetHotLocalDir()
        {
            return GetLocalDir() + "/HotFile";
        }
        /// <summary>
        /// 获取Oss数据主资源本地目录
        /// </summary>
        /// <returns></returns>
        public static string GetOriginalLocalDir()
        {
            return GetLocalDir() + "/OriginalFile";
        }
        /// <summary>
        /// 获取热更本地目录
        /// </summary>
        /// <returns></returns>
        public static string GetLocalDir() 
        {
#if UNITY_EDITOR
            return Application.dataPath.Replace("Assets", "") + "AssetBundle/" + AppData.RunPlatformName ;
#else
            return Application.persistentDataPath + "/AssetBundle/" + AppData.RunPlatformName;
#endif
        }
        /// <summary>
        /// 获取主资源数据的版本地址
        /// </summary>
        /// <returns></returns>
        public static string GetOriginalFilePath(string version,string fullFileName)
        {
            return GetOriginalDir() + $"/{version}/{fullFileName}";
        }
        /// <summary>
        /// 获取主资源数据的版本地址
        /// </summary>
        /// <returns></returns>
        public static string GetOriginalVersionPath()
        {
            return GetOriginalDir() + "/Version.txt";
        }
        /// <summary>
        /// 获取热更数据的版本地址
        /// </summary>
        /// <returns></returns>
        public static string GetHotVersionPath()
        {
            return GetHotDir() + "/Version.txt";
        }
        /// <summary>
        /// 获取热更地址
        /// </summary>
        /// <returns></returns>
        public static string GetHotDir()
        {
            return GetOssDir() + "/HotFile";
        }
        /// <summary>
        /// 获取主资源地址
        /// </summary>
        /// <returns></returns>
        public static string GetOriginalDir()
        {
            return GetOssDir() + "/OriginalFile";
        }
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <returns></returns>
        public static string GetOssDir()
        {
            return AppData.platformType.ToString() + "/MainAssets/"+AppData.RunPlatformName;
        }
    }
}
