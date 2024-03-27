using System.IO;
using UnityEngine;
using YFramework;

namespace Game
{
    public enum LoadAssetType : byte
    {
        GameObject = 1,
    }
    public class PackageBridgeMono : MonoBehaviour
    {
        private PackageBridgeManager mPackageBridgeManager;
        public void SetManager(PackageBridgeManager packageBridgeManager)
        {
            mPackageBridgeManager = packageBridgeManager;
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="data"></param>
        public void LoadAsset(string data)
        {
            if (data == null)
            {
                LogHelper.LogError("LoadAsset 数据为空!");
                return;
            }
            string[] splits = data.Split("&");
            if (splits.Length != 3)
            {
                LogHelper.LogError("LoadAsset 数据格式错误!");
                return;
            }
            GameObject bridge = mPackageBridgeManager.Get(splits[0]);
            string abName = splits[1];
            if (splits[1].Contains("/"))
            {
                abName = splits[1].Substring(splits[1].LastIndexOf("/") + 1);
            }
            LoadAssetType loadAssetType = (LoadAssetType)splits[2].ToByte();
            switch (loadAssetType)
            {
                case LoadAssetType.GameObject:
                    GameObject target = ResourceModule.Load<GameObject>(abName, splits[0]);
                    target .name = splits[1];
                    bridge.SendMessage("LoadGameObject", target, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="data"></param>
        public void LoadScene(string data) 
        {
            if (data == null)
            {
                LogHelper.LogError("LoadScene 数据为空!");
                return;
            }
            string[] splits = data.Split("&");
            if (splits.Length != 2)
            {
                LogHelper.LogError("LoadScene 数据格式错误!");
                return;
            }
            mPackageBridgeManager.StartChangeScene(splits[0]);
            GameCenter.Instance.LoadScene(splits[1]);
        }
        public void GetPinYin(string data)
        {
            if (data == null)
            {
                LogHelper.LogError("GetPinYin 数据为空!");
                return;
            }
            string[] splits = data.Split("&");
            if (splits.Length != 2)
            {
                LogHelper.LogError("GetPinYin 数据格式错误!");
                return;
            }
            char targetChar = splits[1][0];
            char pinyin = PinYinTools.GetHanZiFirstCode(targetChar);
            mPackageBridgeManager.GetPinYin(splits[0], targetChar, pinyin);
        }
    }
}
