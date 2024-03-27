using System.Collections.Generic;
using UnityEngine;
using YFramework;

namespace Game
{
    public class PackageBridgeManager : BaseModule
    {
        private IList<BridgeData> mList;
        private GameObject mBridgeGo;
        public GameObject bridgeGo { get { return mBridgeGo; } }
        public PackageBridgeManager(Center center) : base(center)
        {
        }
        public override void Awake()
        {
            mBridgeGo = new GameObject("LauncherBridge");
            mBridgeGo.DontDestroyOnLoad();
            mBridgeGo.hideFlags = HideFlags.HideAndDontSave;
            mBridgeGo.AddComponent<PackageBridgeMono>().SetManager(this);
            mList = new List<BridgeData>();
        }
        public void Add(BridgeData bridgeManager)
        {
            mList.Add(bridgeManager);
        }
        public GameObject Get(string name)
        {
            for (int i = 0; i < mList.Count; i++)
            {
                if (mList[i].tagName == name) return mList[i].target;
            }
            return null;
        }
        /// <summary>
        /// 开始切换场景
        /// </summary>
        /// <param name="tagName"></param>
        public void StartChangeScene(string tagName)
        {
            GameObject gameObject = Get(tagName);
            if (gameObject == null) 
            {
                LogHelper.LogError("StartChangeScene TagName:" + tagName +"未找到对应的桥接对象");
                return;
            }
            StopGame(tagName);
            gameObject.SendMessage("StartChangeScene",GameCenter.Instance.curScene.sceneName, SendMessageOptions.DontRequireReceiver);
        }
        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="tagName"></param>
        public void StartGame(string tagName)
        {
            GameObject gameObject = Get(tagName);
            if (gameObject == null)
            {
                LogHelper.LogError("StartGame TagName:" + tagName + "未找到对应的桥接对象");
                return;
            }
            gameObject.SendMessage("StartGame",  SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        /// <param name="tagName"></param>
        public void StopGame(string tagName)
        {
            GameObject gameObject = Get(tagName);
            if (gameObject == null)
            {
                LogHelper.LogError("StopGame TagName:" + tagName + "未找到对应的桥接对象");
                return;
            }
            gameObject.SendMessage("StopGame", SendMessageOptions.DontRequireReceiver);
        }
        /// <summary>
        /// 场景切换完成
        /// </summary>
        /// <param name="tagName"></param>
        public void ChangeScene(string tagName)
        {
            GameObject gameObject = Get(tagName);
            if (gameObject == null)
            {
                LogHelper.LogError("ChangeScene TagName:" + tagName + "未找到对应的桥接对象");
                return;
            }
            gameObject.SendMessage("ChangeScene", GameCenter.Instance.curScene.sceneName, SendMessageOptions.DontRequireReceiver);
            StartGame(tagName);
        }
        /// <summary>
        /// 场景切换完成
        /// </summary>
        /// <param name="tagName"></param>
        public void GetPinYin(string tagName,char target,char pinyin )
        {
            GameObject gameObject = Get(tagName);
            if (gameObject == null)
            {
                LogHelper.LogError("GetPinYin TagName:" + tagName + "未找到对应的桥接对象");
                return;
            }
            gameObject.SendMessage("ReceivePinYin", new char[] { target,pinyin  }, SendMessageOptions.DontRequireReceiver);
            StartGame(tagName);
        }
    }
}
