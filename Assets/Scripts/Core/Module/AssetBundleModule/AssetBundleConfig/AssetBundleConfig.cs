using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ABConfig", menuName = "CreateABConfig", order = 50)]
    public class AssetBundleConfig : ScriptableObject
    {
        /// <summary>
        /// 存放所有需要打Asset Bundle包的Prefab的资源目录
        /// </summary>
        public List<string> mAllPrefabDir = new List<string>();

        /// <summary>
        /// 存放所有需要打Asset Bundle包的资源目录
        /// </summary>
        public List<AssetBundleFileDir> mAllFileDir = new List<AssetBundleFileDir>();


        [System.Serializable]
        public struct AssetBundleFileDir
        {
            public string ABName;//Asset Bundle名字
            public string Describute;//Asset Bundle的描述
            public string Path;// AssetBundle文件的目录地址
        }
    } 
}
