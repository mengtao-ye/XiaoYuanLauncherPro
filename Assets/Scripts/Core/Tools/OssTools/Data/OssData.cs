using System.IO;
using UnityEngine;

namespace Game
{
    public static class OssData
    {
        #region Local Path
        /// <summary>
        /// 获取Oss数据主资源本地目录
        /// </summary>
        /// <returns></returns>
        public static string GetLocalHotDir(string packageName)
        {
            return GetLocalDir(packageName) + "/HotFile";
        }
        /// <summary>
        /// 获取Oss数据主资源本地目录
        /// </summary>
        /// <returns></returns>
        public static string GetLocalOriginalDir(string packageName)
        {
            return GetLocalDir(packageName) + "/OriginalFile";
        }
        /// <summary>
        /// 获取热更本地目录
        /// </summary>
        /// <returns></returns>
        public static string GetLocalDir(string packageName)
        {
#if UNITY_EDITOR
            return PathData.ProjectDir + "/AssetBundle/"+ packageName+"/" + AppData.RunPlatformName;
#else
            return Application.persistentDataPath + "/AssetBundle/" + AppData.RunPlatformName;
#endif
        } 
        #endregion
        #region Oss Path

        /// <summary>
        /// 获取主资源数据的版本地址
        /// </summary>
        /// <returns></returns>
        public static string GetOssOriginalFilePath(string version, string fullFileName)
        {
            return GetOssOriginalDir() + $"/{version}/{fullFileName}";
        }

        /// <summary>
        /// 获取主资源数据的版本地址
        /// </summary>
        /// <returns></returns>
        public static string GetOssHotFilePath(string version, string fullFileName)
        {
            return GetOssHotDir() + $"/{version}/{fullFileName}";
        }

        /// <summary>
        /// 获取主资源数据的版本地址
        /// </summary>
        /// <returns></returns>
        public static string GetOssOriginalVersionPath()
        {
            return GetOssOriginalDir() + "/Version.txt";
        }
        /// <summary>
        /// 获取热更数据的版本地址
        /// </summary>
        /// <returns></returns>
        public static string GetOssHotVersionPath()
        {
            return GetOssHotDir() + "/Version.txt";
        }
        /// <summary>
        /// 获取热更地址
        /// </summary>
        /// <returns></returns>
        public static string GetOssHotDir()
        {
            return GetOssDir() + "/HotFile";
        }
        /// <summary>
        /// 获取主资源地址
        /// </summary>
        /// <returns></returns>
        public static string GetOssOriginalDir()
        {
            return GetOssDir() + "/OriginalFile";
        }
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <returns></returns>
        public static string GetOssDir()
        {
           
            return @"http://xiaoyuanapp-oss.oss-cn-hangzhou.aliyuncs.com/" + AppData.platformType.ToString() + "/MainAssets/" + AppData.RunPlatformName;
        } 
        #endregion
    }
}
