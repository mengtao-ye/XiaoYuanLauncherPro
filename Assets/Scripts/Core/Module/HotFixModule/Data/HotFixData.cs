using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Game
{
    public static class HotFixData
    {
#if UNITY_EDITOR
        public static string VersionPath = Application.dataPath+"/../Version" ;//MD5版本地址
        public static string MD5VersionPath = VersionPath +"/MD5_" + PlayerSettings.bundleVersion + ".bytes";//MD5版本信息
        public static string HotPath = Application.dataPath + "/../Hot";
        public static string LocalVersionPathEditor = Application.streamingAssetsPath + "/Version.txt";
#endif
        public static string LocalVersionPath = Application.streamingAssetsPath + "/Version.txt";
        public static string LocalPatchesPath = Application.streamingAssetsPath + "/PatchesVersion.txt";
    }
}


