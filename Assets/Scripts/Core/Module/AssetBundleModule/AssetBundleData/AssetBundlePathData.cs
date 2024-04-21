
using UnityEditor;
using UnityEngine;

namespace Game
{
    public static class AssetBundlePathData
    {
#if UNITY_EDITOR

        /// <summary>
        /// AssetBundle资源打包路径
        /// </summary>
        public static string ASSETBUNDLE_PATH_EDITOR = Application.dataPath.Replace("/Assets","") + "/AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString();
        /// <summary>
        /// AB包文件资源
        /// </summary>
        public static string ASSETBUNDLE_FILE_PATH = ASSETBUNDLE_PATH_EDITOR + "/ABFileConfig.bytes";
        /// <summary>
        /// AB热更资源包包文件
        /// </summary>
        public static string ASSETBUNDLE_HOT_FILE_PATH = ASSETBUNDLE_PATH_EDITOR + "/HotABFileConfig.bytes";
        /// <summary>
        /// 配置表路径
        /// </summary>
        public static string ASSETBUNDLE_CONFIG_PATH_EDITOR = "Assets/Editor/AssetBundle/Config/ABConfig.asset";
        /// <summary>
        ///Xml数据类型配置表地址
        /// </summary>
        public static string ASSETBUNDLE_XML_CONFIG_EDITOR = "Assets/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/Config/XmlABConfig.xml";
        /// <summary>
        /// 字节数组类型配置表地址
        /// </summary>
        public static string ASSETBUNDLE_BINARY_CONFIG_EDITOR = "Assets/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/Config/BinaryABConfig.bytes";
        /// <summary>
        /// 配置表目录
        /// </summary>
        public static string ASSETBUNDLE_CONFIG_DATA_PATH_EDITOR = "Assets/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/Config";
#endif
   
        /// <summary>
        /// 打包后读取的Asset Bundle地址
        /// </summary>
        public static string ASSETBUNDLE_PATH =
#if UNITY_EDITOR
        ASSETBUNDLE_PATH_EDITOR;
#elif UNITY_ANDROID
                Application.persistentDataPath + "/AssetBundle/"+AppData.RunPlatformName ;
#endif
    }
}
