using UnityEngine;
using YFramework;

namespace Game
{
    public class AssetBundleItem : BaseCount,IPool
    {
        public AssetBundle assetBundle = null;
        public ulong CRC = 0;
        public bool isPop { get ; set ; }
        private string managerName;//管理器名称

        public void SetManagerName(string name)
        {
            managerName = name;    
        }

        public void PopPool()
        {
        }
        public void PushPool()
        {
            assetBundle = null;
            CRC = 0;
        }

        public void Recycle()
        {
            ClassPool<AssetBundleItem>.Push(this);
        }
        protected override void Enter()
        {
           
        }
        protected override void Exit()
        {
            if (assetBundle != null) 
            {
                assetBundle.Unload(true);
            }
            if ( AssetBundleModule.Get(managerName).assetBundleItemDict.ContainsKey(CRC)) {
                AssetBundleModule.Get(managerName).assetBundleItemDict.Remove(CRC);
            }
            ClassPool<AssetBundleItem>.Push(this);
        }
    }
}
