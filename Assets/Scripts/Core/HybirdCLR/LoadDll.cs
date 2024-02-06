using HybridCLR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Game
{
    public class LoadDll
    {
        private Dictionary<string, byte[]> mAssetBytesDataDict;

        private List<string> AOTMetaAssemblyNames;
        public Assembly mainAssembly { get; private set; }
        public LoadDll()
        {
            mAssetBytesDataDict = new Dictionary<string, byte[]>();
            AOTMetaAssemblyNames = new List<string>();
            AOTMetaAssemblyNames.Add("mscorlib.dll");
            AOTMetaAssemblyNames.Add("System.dll");
            AOTMetaAssemblyNames.Add("System.Core.dll");
        }
        
        public void Init(string dllFullName,byte[] data)
        {
            mainAssembly = EditorLoadHotUpdateDllAssembly(dllFullName, data);
        }
        private Assembly EditorLoadHotUpdateDllAssembly(string dllName,byte[] data)
        {
            AOTMetaAssemblyNames.Add(dllName);
            AssetBundle ab = AssetBundle.LoadFromMemory(data);
            foreach (var aotDllName in AOTMetaAssemblyNames)
            {
                mAssetBytesDataDict.Add(aotDllName, ab.LoadAsset<TextAsset>(aotDllName).bytes);
            }
            /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in AOTMetaAssemblyNames)
            {
                byte[] dllBytes = mAssetBytesDataDict[aotDllName];
                // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
                Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
            }
            return Assembly.Load(mAssetBytesDataDict[dllName]);
        }
    }
}
