using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public static class ResourceModule
    {
        private static Dictionary<ulong, ResourceItem> mResourceItemDict = new Dictionary<ulong, ResourceItem>();
        public static Dictionary<ulong, ResourceItem> resourceItemDict { get { return mResourceItemDict; } }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"> 加载的数据类型 </typeparam>
        /// <param name="assetName">加载的对象路径 eg:Assets/GameData/Audio.mp3</param>
        /// <returns></returns>
        public static T Load<T>(string assetName,string managerName) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(assetName)) return default(T);
            if (AppConstData.UseABLoad)
            {
                ResourceItem resource = null;
                ulong crc = CRC32Tool.GetCRC32(assetName);
                if (!mResourceItemDict.ContainsKey(crc))
                {
                    resource = AssetBundleModule.Get(managerName).LoadResourceItem(crc, managerName);
                    mResourceItemDict.Add(crc, resource);
                    resource.AsyncLoad = false;
                }
                else
                {
                    resource = mResourceItemDict[crc];
                }
                if (resource == null) return null;
                resource.count++;
                resource.LastUseTime = UnityEngine.Time.realtimeSinceStartup;
                return resource.Obj as T;
            }
            else 
            {
#if UNITY_EDITOR
                return AssetDatabase.LoadAssetAtPath<T>(assetName);
#endif
            }
            return null;
        }
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">加载的资源类型</typeparam>
        /// <param name="path">加载的资源地址</param>
        /// <param name="Complete">加载完成后执行的回调</param>
        public static void LoadAsync<T>(string path,Action<T> Complete) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path)) return;
#if !UNITY_EDITOR

            ulong crc = CRC32Tool.GetCRC32(path);
            ResourceItem resource = null;
            if (!mResourceItemDict.ContainsKey(crc))
            {
                resource = AssetBundleModule.LoadResourceItem(crc);
                resource.AsyncLoad = true;
                resource.Finish = (obj) =>
                {
                    Complete(obj as T);
                };
                mResourceItemDict.Add(crc, resource);
            }
            else
            {
                resource = mResourceItemDict[crc];
                if(resource.Obj !=null)
                    Complete(resource.Obj as T);
            }
            if (resource == null) return;
            resource.LastUseTime = UnityEngine.Time.realtimeSinceStartup;
            resource.count++;
#else
            T tempData =  AssetDatabase.LoadAssetAtPath<T>(path);
            Complete.Invoke(tempData) ;
#endif
        }
        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="forceUnload">是否直接卸载资源（改选项可能会导致其他对象引用丢失）</param>
        public static void Release(string path,bool forceUnload = false)
        {
            if (string.IsNullOrEmpty(path)) return;
            ulong crc = CRC32Tool.GetCRC32(path);
            ResourceItem item = mResourceItemDict.TryGet(crc);
            if (item == null) 
            {
                return;
            }
            if (forceUnload)
            {
                //执行卸载方法
                item.count -= int.MaxValue;
            }
            else {
                item.count--;
            }
        
        }
    }
}
