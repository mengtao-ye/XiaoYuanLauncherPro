using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YFramework;
using static YFramework.Core.Utility.Utility;
using static YFramework.Utility;

namespace Game
{
    public static class HotFixModule
    {
        private static float mVersion = -1;//当前版本信息
        private static bool singlePatchFinish = false;//单个补丁包是否下载完成
        private static string downLoadPatchName = null;//单个补丁包下载完成
        /// <summary>
        /// 检查热更
        /// </summary>
        public static void CheckHotFix()
        {
            if (mVersion == -1)
            {
                mVersion = ReadVersion();
            }
            if (mVersion == -1)
            {
                Debug.LogError("未读取到当前版本信息！");
                return;
            }
            Dictionary<string, byte[]> data = new Dictionary<string, byte[]>();
            data.Add("Version", BinaryMapper.GetBytes(mVersion.ToString()));
            string patchesVersion = "";
            if (File.Exists(HotFixData.LocalPatchesPath))
            {
                patchesVersion = File.ReadAllText(HotFixData.LocalPatchesPath).Split('|')[1];
                data.Add("PatchesVerion", BinaryMapper.GetBytes(patchesVersion));
            }
            //TODO
            //GameCenter.Instance.mSocketManager.TcpSend((short)ActionCode.GetPatchs, data, ReadPatches);
        }
        /// <summary>
        /// 读取补丁包信息
        /// </summary>
        /// <param name="data"></param>
        private static void ReadPatches(Dictionary<string, byte[]> data)
        {
            if (!data.ContainsKey("Patches")) return;
            Patches patches = XmlMapper.ToObject<Patches>(data.TryGet("Patches").ToStr());
            DirectoryInfo directoryInfo = new DirectoryInfo(AssetBundlePathData.ASSETBUNDLE_PATH);
            FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            List<Patch> downLoadList = new List<Patch>();
            for (int i = 0; i < patches.Files.Count; i++)
            {
                if (CalcDownLoad(files, patches.Files[i]))
                {
                    downLoadList.Add(patches.Files[i]);
                }
            }
            IEnumeratorModule.StartCoroutine(DownLoadPatch(patches.Version, downLoadList));
        }
        /// <summary>
        /// 补丁包下载
        /// </summary>
        /// <param name="patchList"></param>
        /// <returns></returns>
        private static System.Collections.IEnumerator DownLoadPatch(string patchVersion, List<Patch> patchList)
        {
            for (int i = 0; i < patchList.Count; i++)
            {
                singlePatchFinish = false;
                //TODO
                //GameCenter.Instance.mSocketManager.TcpSend( (short)ActionCode.DownLoadPatchs,"Url",BinaryMapper.GetBytes(patchList[i].Url),(item)=> {
                //    byte[] patchByteData = item.TryGet("Data");
                //    if (string.IsNullOrEmpty(downLoadPatchName) || patchByteData == null|| patchByteData.Length == 0) return;
                //    string path = AssetBundleData.ASSETBUNDLE_PATH + "/" + downLoadPatchName;
                //    if (File.Exists(path)) File.Delete(path);
                //    File.Create(path).Dispose();
                //    File.WriteAllBytes(path, patchByteData);
                //    singlePatchFinish = true;
                //});
                downLoadPatchName = patchList[i].Name;
                if (!singlePatchFinish)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForEndOfFrame();
            string dir = Path.GetDirectoryName(HotFixData.LocalPatchesPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(HotFixData.LocalPatchesPath)) File.Create(HotFixData.LocalPatchesPath).Dispose();
            File.WriteAllText(HotFixData.LocalPatchesPath, "PatchesVersion|" + patchVersion);
            Debug.Log("补丁包下载完成");
        }
        /// <summary>
        /// 判断是否需要下载
        /// </summary>
        /// <param name="files"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        private static bool CalcDownLoad(FileInfo[] files, Patch patch)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name == patch.Name)
                {
                    string md5 = MD5Tool.Md5(files[i].FullName);
                    if (md5 == patch.MD5)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 读取版本信息
        /// </summary>
        private static float ReadVersion()
        {
            if (!File.Exists(HotFixData.LocalVersionPath))
            {
                Debug.LogError(string.Format("本地版本地址：{0} 不存在!", HotFixData.LocalVersionPath));
                return -1;
            }
            string message = File.ReadAllText(HotFixData.LocalVersionPath);
            string[] versionStrs = message.Split('|');
            float version = 0;
            if (versionStrs.Length == 2 && float.TryParse(versionStrs[1], out version))
            {
                return version;
            }
            Debug.LogError(string.Format("本地版本地址：{0} 信息异常!", HotFixData.LocalVersionPath));
            return -1;
        }
    }
}
