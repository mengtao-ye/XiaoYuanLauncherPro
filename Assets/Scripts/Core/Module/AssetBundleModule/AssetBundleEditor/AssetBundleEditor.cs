#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YFramework;
using static YFramework.Core.Utility.Utility;
using static YFramework.Utility;

namespace Game
{
    public static class AssetBundleEditor
    {
        /// <summary>
        /// 所有需要打包的文件的字典，key为AB包名，value为AB包地址
        /// </summary>
        private static Dictionary<string, string> mAllFileDict = new Dictionary<string, string>();
        /// <summary>
        /// 存放所有需要打包的文件地址,计算冗余
        /// </summary>
        private static List<string> mAllFileList = new List<string>();
        /// <summary>
        /// 所有Prefab对用的资源文件 ,key 为Prefab的名字，value为改Prefab所依赖的资源
        /// </summary>
        private static Dictionary<string, List<string>> mAllPrefabDependenceDict = new Dictionary<string, List<string>>();
        /// <summary>
        /// 冗余不需要写入XmlConfig的文件
        /// </summary>
        private static List<string> mVaildPathList = new List<string>();
        [MenuItem("EditorTools/AssetBundle/NormalPackageAssetBundle")]
        private static void NormalPackageAssetBundle()
        {
            PackageAssetBundle(false, null, "0.0.0");
        }
        public static void PackageAssetBundle(bool isHotFix, string hotFixPath, string hotFixVersion)
        {
            int versionCode = 0;
            if (!isHotFix)
            {
                versionCode = AppTools.GetVersionCode(PlayerSettings.bundleVersion);
            }
            else
            {
                versionCode =AppTools. GetVersionCode(hotFixVersion);
            }
            if (versionCode == 0) return;
            mVaildPathList.Clear();
            mAllFileDict.Clear();
            mAllFileList.Clear();
            mAllPrefabDependenceDict.Clear();
            AssetBundleConfig assetBundleConfig = UnityEditor.AssetDatabase.LoadAssetAtPath<AssetBundleConfig>(AssetBundlePathData.ASSETBUNDLE_CONFIG_PATH_EDITOR);
            for (int i = 0; i < assetBundleConfig.mAllFileDir.Count; i++)
            {
                if (mAllFileDict.ContainsKey(assetBundleConfig.mAllFileDir[i].ABName))//检查是否有同名的AB包名
                {
                    Debug.LogError("AB包名重复,ABName:" + assetBundleConfig.mAllFileDir[i].ABName);
                    return;
                }
                else
                {

                    mVaildPathList.Add(assetBundleConfig.mAllFileDir[i].Path);
                    mAllFileList.Add(assetBundleConfig.mAllFileDir[i].Path);
                    mAllFileDict.Add(assetBundleConfig.mAllFileDir[i].ABName, assetBundleConfig.mAllFileDir[i].Path);
                }
            }
            string[] prefabsPath = AssetDatabase.FindAssets("t:Prefab", assetBundleConfig.mAllPrefabDir.ToArray());
            for (int i = 0; i < prefabsPath.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(prefabsPath[i]);
                mVaildPathList.Add(path);
                EditorUtility.DisplayProgressBar("查找Prefab", path, (i * 1f) / prefabsPath.Length);
                string[] allDependences = AssetDatabase.GetDependencies(path);
                List<string> allDependenceList = new List<string>();
                for (int j = 0; j < allDependences.Length; j++)
                {
                    if (!IsInAllFile(allDependences[j]) && !allDependences[j].EndsWith(".cs"))
                    //判断Prefab依赖的文件是否已经存在在AB资源文件中并且不是脚本文件的情况下
                    {
                        mAllFileList.Add(allDependences[j]);
                        allDependenceList.Add(allDependences[j]);
                    }
                }
                string fileName = Path.GetFileNameWithoutExtension(path);
                if (mAllPrefabDependenceDict.ContainsKey(fileName))
                {
                    Debug.LogError("Prefab名字重复，Name：" + fileName);
                    return;
                }
                else
                {
                    mAllPrefabDependenceDict.Add(fileName, allDependenceList);
                }
            }
            EditorUtility.ClearProgressBar();
            //设置文件的AB包名
            foreach (var item in mAllFileDict)
            {
                SetABName(item.Key, item.Value);
            }
            //设置Prefab的AB包名
            foreach (var item in mAllPrefabDependenceDict)
            {
                SetABName(item.Key, item.Value);
            }
            DeleteUnuseAB();
            BuildAssetBundle();
            ClearABName();
            AssetDatabase.Refresh();
            BuildAssetBundleConfig();
            if (!isHotFix)
            {
                BuildFileConfig(AssetBundlePathData.ASSETBUNDLE_FILE_PATH, versionCode);
                HotFixEditor.WriteABMD5();
            }
            else
            {
                BuildFileConfig(AssetBundlePathData.ASSETBUNDLE_HOT_FILE_PATH, versionCode);
                HotFixEditor.ReadMD5Com(hotFixPath, hotFixVersion);
            }
            HotFixEditor.WriteVersion();
            //SpawnAssetBundle(); //生成一份测试数据到本地
            Debug.Log("AssetBundle Build Finish!");
        }

        

        /// <summary>
        /// 构建所有ab包文件配置表
        /// </summary>
        private static void BuildFileConfig(string savePath, int version)
        {
            string[] manifest = Directory.GetFiles(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR, "*.manifest", SearchOption.AllDirectories);
            if (!manifest.IsNullOrEmpty())
            {
                for (int i = 0; i < manifest.Length; i++)
                {
                    File.Delete(manifest[i]);
                }
            }
            EditorUtility.DisplayProgressBar("构建配置表", "构建所有ab包文件配置表", 0);
            if (!Directory.Exists(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR))
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError("AB包资源文件夹不存在：" + AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR);
                return;
            }
            string[] files = Directory.GetFiles(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR, "*.*", SearchOption.AllDirectories);
            AssetBundleFileData assetBundleFileData = new AssetBundleFileData();
            assetBundleFileData.version = version;
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    assetBundleFileData.size += fileInfo.Length;
                    assetBundleFileData.fileNames.Add(fileInfo.Name);
                }
            }
            File.WriteAllBytes(savePath, assetBundleFileData.ToBytes());
            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 把配置文件打成AB包
        /// </summary>
        private static void BuildAssetBundleConfig()
        {
            string mTempPath = Application.dataPath + "/BuildABTempDir";
            if (Directory.Exists(mTempPath))
            {
                Directory.Delete(mTempPath, true);
            }
            Directory.Move(AssetBundlePathData.ASSETBUNDLE_CONFIG_DATA_PATH_EDITOR, mTempPath);
            AssetDatabase.Refresh();
            mTempPath = mTempPath.Replace(Application.dataPath, "");
            mTempPath = "Assets" + mTempPath;
            SetABName("config", mTempPath);
            if (DirectoryTools.CheckFileIsIn(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR, "config"))
            {
                Debug.LogError(string.Format("文件夹{0}已存在config文件", AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR));
                return;
            }
            BuildPipeline.BuildAssetBundles(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            //Directory.Delete(mTempPath, true);
            ClearABName();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 删除没用的AB包
        /// </summary>
        private static void DeleteUnuseAB()
        {
            string[] allAssetBundle = AssetDatabase.GetAllAssetBundleNames();
            if (!Directory.Exists(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR)) Directory.CreateDirectory(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR);
            DirectoryInfo directoryInfo = new DirectoryInfo(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR);
            FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (!IsInAssetBundlePath(allAssetBundle, files[i].Name) && files[i].Extension != ".meta")
                {
                    File.Delete(files[i].FullName);
                }
            }
        }
        /// <summary>
        /// 判断AB包是否已经存在
        /// </summary>
        /// <param name="allAssetBunle"></param>
        /// <param name="abName"></param>
        /// <returns></returns>
        private static bool IsInAssetBundlePath(string[] allAssetBunle, string abName)
        {
            if (string.IsNullOrEmpty(abName)) return false;
            if (allAssetBunle.IsNullOrEmpty()) return false;
            for (int i = 0; i < allAssetBunle.Length; i++)
            {
                if (allAssetBunle[i] == abName) return true;
            }
            return false;
        }
        /// <summary>
        /// 打包Asset Bundle
        /// </summary>
        private static void BuildAssetBundle()
        {
            string[] allAssetBundle = AssetDatabase.GetAllAssetBundleNames();
            //存放所有的AssetBundle资源路径 ，key 为地址，value为AB包名
            Dictionary<string, string> allAssetBundleDict = new Dictionary<string, string>();
            for (int i = 0; i < allAssetBundle.Length; i++)
            {
                string[] allAssetBundlePath = AssetDatabase.GetAssetPathsFromAssetBundle(allAssetBundle[i]);
                for (int j = 0; j < allAssetBundlePath.Length; j++)
                {
                    if (!allAssetBundlePath[j].EndsWith(".cs") && VaildPath(allAssetBundlePath[j]))
                    {
                        allAssetBundleDict.Add(allAssetBundlePath[j], allAssetBundle[i]);
                    }
                }
            }
            if (!Directory.Exists(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR))
            {
                Directory.CreateDirectory(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR);
            }
            WriteAssetBundleConfigData(allAssetBundleDict);
            BuildPipeline.BuildAssetBundles(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        }
        private static void WriteAssetBundleConfigData(Dictionary<string, string> allAssetBundleDict)
        {
            AssetBundleConfigData assetBundleConfigData = new AssetBundleConfigData();
            assetBundleConfigData.ABList = new List<ABBase>();
            foreach (var item in allAssetBundleDict)
            {
                ABBase abBase = new ABBase();
                abBase.ABName = item.Value;
                abBase.Path = item.Key;
                abBase.AssetName = Path.GetFileNameWithoutExtension(item.Key);
                abBase.CRC = CRC32Tool.GetCRC32(abBase.AssetName);
                abBase.Dependence = new List<string>();
                string[] tempAllDependence = AssetDatabase.GetDependencies(item.Key);
                for (int i = 0; i < tempAllDependence.Length; i++)
                {
                    //如果这个资源的依赖项是自己或者是脚本的话就不进行下去
                    if (tempAllDependence[i] == item.Key || Path.GetExtension(tempAllDependence[i]) == ".cs") continue;
                    string tempDependenceAssetBundleName = allAssetBundleDict.TryGet(tempAllDependence[i]);
                    if (string.IsNullOrEmpty(tempDependenceAssetBundleName)) continue;
                    if (abBase.Dependence.Contains(tempDependenceAssetBundleName)) continue;
                    abBase.Dependence.Add(tempDependenceAssetBundleName);
                }
                assetBundleConfigData.ABList.Add(abBase);
            }
            //写入Xml
            FileTools.Write(AssetBundlePathData.ASSETBUNDLE_XML_CONFIG_EDITOR, XmlMapper.ToXml(assetBundleConfigData));
            //写入Byte
            for (int i = 0; i < assetBundleConfigData.ABList.Count; i++)
            {
                assetBundleConfigData.ABList[i].Path = "";
            }
            FileTools.Write(AssetBundlePathData.ASSETBUNDLE_BINARY_CONFIG_EDITOR, assetBundleConfigData.ToBytes());
        }
        /// <summary>
        /// 清理所有设置AB包名的资源
        /// </summary>
        private static void ClearABName()
        {
            string[] allPath = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < allPath.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(allPath[i], true);
            }
        }
        /// <summary>
        /// 设置AB包名
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="path"></param>
        private static void SetABName(string abName, string path)
        {
            if (string.IsNullOrEmpty(abName) || string.IsNullOrEmpty(path)) return;
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if (assetImporter == null)
            {
                Debug.LogError("设置AB包地址错误,Path:" + path);
                return;
            }
            assetImporter.assetBundleName = abName;
        }
        /// <summary>
        /// 设置AB包名
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="path"></param>
        private static void SetABName(string abName, List<string> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                SetABName(abName, path[i]);
            }
        }
        /// <summary>
        /// 判断地址是否在需要打包的AB包资源文件目录中
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsInAllFile(string path)
        {
            for (int i = 0; i < mAllFileList.Count; i++)
            {
                if (mAllFileList[i] == path || (path.Contains(mAllFileList[i]) && path.Replace(mAllFileList[i], "")[0] == '/'))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断地址是否需要写入XmlConfig
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool VaildPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Path is NULL");
                return false;
            }
            for (int i = 0; i < mVaildPathList.Count; i++)
            {
                if (path.Contains(mVaildPathList[i])) return true;
            }
            return false;
        }
    }
}
#endif