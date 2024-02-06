using Aliyun.OSS;
using Aliyun.OSS.Common;
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
        private OssClient mOssClient;
        public AliyunOSSTools()
        {
            mOssClient = new OssClient(OssConfig.EndPoint, OssConfig.AccessKeyId, OssConfig.AccessKeySecret);
        }
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
                Log.LogError("LoadFileConfig data is empty or null");
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
        /// <summary>
        /// 加载oss文件(加密)
        /// </summary>
        /// <param name="ossPath"></param>
        /// <param name="savePath"></param>
        /// <param name="loadProcess"></param>
        /// <param name="loadSuccss"></param>
        /// <param name="loadFail"></param>
        public void LoadOssFileToLocal(string ossPath, string savePath, Action<float> loadProcess = null, Action<long> loadSuccss = null, Action<string> loadFail = null)
        {
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest(OssConfig.Bucket, ossPath);
                getObjectRequest.StreamTransferProgress = (sender, args) =>
                {
                    float process = (args.TransferredBytes * 100 / args.TotalBytes) / 100.0f;
                    loadProcess?.Invoke(process);
                };
                FileTools.CreateFile(savePath);
                OssObject result = mOssClient.GetObject(getObjectRequest);
                using (var resultStream = result.Content)
                {
                    byte[] buf = new byte[1024];
                    var fs = File.Open(savePath, FileMode.OpenOrCreate);
                    var len = 0;
                    bool isFirst = false;
                    long size = 0;
                    while ((len = resultStream.Read(buf, 0, 1024)) != 0)
                    {
                        if (!isFirst)
                        {
                            EncryptionTools.Encryption(buf);
                            isFirst = true;
                        }
                        size += len;
                        fs.Write(buf, 0, len);
                    }
                    fs.Close();
                    loadSuccss?.Invoke(size);
                }
            }
            catch (OssException e)
            {
                Debug.LogError("进度下载文件出错：" + e.Message);
                loadFail?.Invoke(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("进度下载文件出错：" + e.Message);
                loadFail?.Invoke(e.Message);
            }
        }
        /// <summary>
        /// 加载oss文件（未加密）
        /// </summary>
        /// <param name="ossPath"></param>
        /// <param name="savePath"></param>
        /// <param name="loadProcess"></param>
        /// <param name="loadSuccss"></param>
        /// <param name="loadFail"></param>
        public void LoadOssBytes(string ossPath, Action<float> loadProcess = null, Action<byte[]> loadSuccss = null, Action<string> loadFail = null)
        {
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest(OssConfig.Bucket, ossPath);
                getObjectRequest.StreamTransferProgress = (sender, args) =>
                {
                    float process = (args.TransferredBytes * 100 / args.TotalBytes) / 100.0f;
                    loadProcess?.Invoke(process);
                };
                OssObject result = mOssClient.GetObject(getObjectRequest);
                using (var resultStream = result.Content)
                {
                    byte[] loadBytes = new byte[result.Content.Length];
                    byte[] buf = new byte[1024];
                    var len = 0;
                    long size = 0;
                    while ((len = resultStream.Read(buf, 0, 1024)) != 0)
                    {
                        for (int i = 0; i < len; i++)
                        {
                            loadBytes[size + i] = buf[i];
                        }
                        size += len;
                    }
                    loadSuccss?.Invoke(loadBytes);
                }
            }
            catch (OssException e)
            {
                Debug.LogError("进度下载文件出错：" + e.Message);
                loadFail?.Invoke(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("进度下载文件出错：" + e.Message);
                loadFail?.Invoke(e.Message);
            }
        }
        /// <summary>
        /// 加载oss文件（未加密）
        /// </summary>
        /// <param name="ossPath"></param>
        /// <param name="savePath"></param>
        /// <param name="loadProcess"></param>
        /// <param name="loadSuccss"></param>
        /// <param name="loadFail"></param>
        public void LoadOssString(string ossPath, Action<float> loadProcess , Action<string> loadSuccss, Action<string> loadFail)
        {
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest(OssConfig.Bucket, ossPath);
                getObjectRequest.StreamTransferProgress = (sender, args) =>
                {
                    float process = (args.TransferredBytes * 100 / args.TotalBytes) / 100.0f;
                    loadProcess?.Invoke(process);
                };
                OssObject result = mOssClient.GetObject(getObjectRequest);
                using (var resultStream = result.Content)
                {
                    byte[] loadBytes = new byte[result.Content.Length];
                    byte[] buf = new byte[1024];
                    var len = 0;
                    long size = 0;
                    while ((len = resultStream.Read(buf, 0, 1024)) != 0)
                    {
                        for (int i = 0; i < len; i++)
                        {
                            loadBytes[size + i] = buf[i];
                        }
                        size += len;
                    }
                    loadSuccss?.Invoke(Encoding.UTF8.GetString(loadBytes));
                }
            }
            catch (OssException e)
            {
                Debug.LogError("进度下载文件出错：" + e.Message);
                loadFail?.Invoke(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError("进度下载文件出错：" + e.Message);
                loadFail?.Invoke(e.Message);
            }
        }
    }
}
