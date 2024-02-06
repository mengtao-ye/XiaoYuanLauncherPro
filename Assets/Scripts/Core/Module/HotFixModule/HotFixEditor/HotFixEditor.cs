#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YFramework;
using static YFramework.Core.Utility.Utility;
using static YFramework.Utility;

namespace Game
{
    public static class HotFixEditor
    {
        /// <summary>
        /// 所有补丁包 ，key存储的是AB包名，value存储的是MD5信息
        /// </summary>
        private static Dictionary<string, BaseMD5> mAllMD5Dict = new Dictionary<string, BaseMD5>();

        /// <summary>
        /// 写入版本信息到本地
        /// </summary>
        public static void WriteVersion()
        {
            string dir = Path.GetDirectoryName(HotFixData.LocalVersionPathEditor);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(HotFixData.LocalVersionPathEditor))
            {
                File.Delete(HotFixData.LocalVersionPathEditor);
            }
            File.Create(HotFixData.LocalVersionPathEditor).Dispose();
            string message = "Version|" + PlayerSettings.bundleVersion;
            File.WriteAllText(HotFixData.LocalVersionPathEditor, message);
        }
        /// <summary>
        /// 读取MD5数据
        /// </summary>
        /// <param name="md5Path"></param>
        /// <param name="hotVersion"></param>
        public static void ReadMD5Com(string md5Path, string hotfixVersion)
        {
            mAllMD5Dict.Clear();
            byte[] datas = File.ReadAllBytes(md5Path);
            ABMD5 abMD5 = BinaryMapper.ToObject<ABMD5>(datas);
            for (int i = 0; i < abMD5.MD5List.Count; i++)
            {
                mAllMD5Dict.Add(abMD5.MD5List[i].Name, abMD5.MD5List[i]);
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR);
            FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            List<string> changeABPackage = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Name.EndsWith(".manifest") && !files[i].Name.EndsWith(".meta"))
                {
                    string name = files[i].Name;
                    string md5 = MD5Tool.Md5(files[i].FullName);
                    if (!mAllMD5Dict.ContainsKey(name))
                    {
                        changeABPackage.Add(name);
                    }
                    else
                    {
                        if (mAllMD5Dict[name].MD5 != md5)
                        {
                            changeABPackage.Add(name);
                        }
                    }
                }
            }
            CopyABAndGeneratorXml(changeABPackage, hotfixVersion);
        }
        /// <summary>
        /// 生成补丁包资源及补丁包配置表
        /// </summary>
        /// <param name="changeName"></param>
        /// <param name="hotVersion"></param>
        private static void CopyABAndGeneratorXml(List<string> changeName, string hotVersion)
        {
            if (!Directory.Exists(HotFixData.HotPath))
            {
                Directory.CreateDirectory(HotFixData.HotPath);
            }
            DirectoryTools.ClearDir(HotFixData.HotPath);
            string savePath = HotFixData.HotPath + "/" + PlayerSettings.bundleVersion + "/" + hotVersion;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            else
            {
                DirectoryTools.ClearDir(savePath);
            }
            for (int i = 0; i < changeName.Count; i++)
            {
                if (!changeName[i].EndsWith(".manifest"))
                {

                    File.Copy(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR + "/" + changeName[i], savePath + "/" + changeName[i]);
                }
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(HotFixData.HotPath);
            FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            Patches patches = new Patches();
            patches.Version = hotVersion;
            patches.Describute = "测试";
            patches.Files = new List<Patch>();
            for (int i = 0; i < files.Length; i++)
            {
                Patch patch = new Patch();
                patch.Name = files[i].Name;
                patch.MD5 = MD5Tool.Md5(files[i].FullName);
                patch.Platform = EditorUserBuildSettings.activeBuildTarget.ToString();
                patch.Size = files[i].Length / 1024.0f;
                patch.Url = PlayerSettings.bundleVersion + "/" + hotVersion + "/" + files[i].Name;
                patches.Files.Add(patch);
            }
            string path = HotFixData.HotPath + "/" + PlayerSettings.bundleVersion + "/Patchs.xml";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.Create(path).Dispose();
            string content = XmlMapper.ToXml(patches);
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// 写入MD5码
        /// </summary>
        public static void WriteABMD5()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(AssetBundlePathData.ASSETBUNDLE_PATH_EDITOR);
            FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            ABMD5 abMD5 = new ABMD5();
            abMD5.MD5List = new List<BaseMD5>();
            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Name.EndsWith(".manifest") && !files[i].Name.EndsWith(".meta"))
                {
                    BaseMD5 md5Base = new BaseMD5();
                    md5Base.Name = files[i].Name;
                    md5Base.MD5 = MD5Tool.Md5(files[i].FullName);
                    md5Base.Size = files[i].Length / 1024.0f;
                    abMD5.MD5List.Add(md5Base);
                }
            }
            if (!Directory.Exists(HotFixData.VersionPath))
            {
                Directory.CreateDirectory(HotFixData.VersionPath);
            }
            if (File.Exists(HotFixData.MD5VersionPath))
            {
                File.Delete(HotFixData.MD5VersionPath);
            }
            File.Create(HotFixData.MD5VersionPath).Close();
            FileTools.Write(HotFixData.MD5VersionPath, BinaryMapper.ToBinary(abMD5));
        }
    }
}

#endif