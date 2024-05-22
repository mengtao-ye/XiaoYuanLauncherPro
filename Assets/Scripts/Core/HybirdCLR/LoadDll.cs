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
            AssetBundle ab = AssetBundle.LoadFromMemory(data);
            foreach (var aotDllName in AOTMetaAssemblyNames)
            {
                mAssetBytesDataDict.Add(aotDllName, ab.LoadAsset<TextAsset>(aotDllName).bytes);
            }
            /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
            /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in AOTMetaAssemblyNames)
            {
                byte[] dllBytes = mAssetBytesDataDict[aotDllName];
                // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
                Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
            }
            Assembly assembly = Assembly.Load(ab.LoadAsset<TextAsset>(dllName).bytes);
            Debug.Log("dllName:"+ dllName+ ",assembly.FullName:"+ assembly.FullName);
            Type type = assembly.GetType("Game.UniqueCodeGenerator");
            MethodInfo method = type.GetMethod("GenerateUniqueCode");
             Debug.Log( method.Invoke(null,null));
            return assembly;
        }
    }
}
