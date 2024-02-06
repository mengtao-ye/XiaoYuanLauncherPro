using System;
using System.Collections.Generic;
using UnityEngine;
using YFramework;

namespace Game
{
    public class ResourceItem : BaseCount
    {
        public string ABName { get; set; } = null;
        public string AssetName { get; set; } = null;
        public ulong CRC { get; set; } = 0;
        public List<string> Dependence { get; set; } = null;
        public AssetBundle AssetBundle { get; set; } = null;
        public UnityEngine.Object Obj { get; set; } = null;
        public float LastUseTime { get; set; } = 0;
        public Action<UnityEngine.Object> Finish { get; set; } = null;
        public bool AsyncLoad = false;
        private string managerName;

        public ResourceItem(string name) {
            managerName = name;
        }

        protected override void Enter()
        {
            if (Obj == null && AssetBundle != null)
            {
                if (AsyncLoad)
                {
                    if (Finish == null) {
                        Debug.LogError("请注册Finish的回调方法！");
                        return;
                    }
                    IEnumeratorModule.StartCoroutine(IELoadAssetBundle());
                }
                else
                {
                    Obj = AssetBundle.LoadAsset(AssetName);
                }
            }
        }
        protected override void Exit()
        {
            if (Obj != null) {
                if (Obj is GameObject)
                    Resources.UnloadUnusedAssets();
                else
                    Resources.UnloadAsset(Obj);
                Obj = null;
            }
            AssetBundleModule.Get(managerName).ReleaseResource(this);
            if (ResourceModule.resourceItemDict.ContainsKey(CRC))
            {
                ResourceModule.resourceItemDict.Remove(CRC);
            }
        }
        private System.Collections.IEnumerator IELoadAssetBundle() {
            AssetBundleRequest assetBundleRequest = AssetBundle.LoadAssetAsync(AssetName);
            yield return assetBundleRequest;
            Obj = assetBundleRequest.asset;
            Finish(assetBundleRequest.asset);
        }
    }
}
