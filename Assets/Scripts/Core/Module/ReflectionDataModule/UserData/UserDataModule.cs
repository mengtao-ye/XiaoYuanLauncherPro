using System;
using System.Collections.Generic;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 用户数据模块
    /// </summary>
    public static class UserDataModule
    {
        private static Dictionary<int, Queue<IUnityUserDataCallBack>> mUserDataQueueCallBackDict  = new Dictionary<int, Queue<IUnityUserDataCallBack>>();
        private static Dictionary<int, UnityUserData> mUserDataDict = new Dictionary<int, UnityUserData>();
        /// <summary>
        /// 映射用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="image"></param>
        /// <param name="name"></param>
        public static void MapUserData<T>(int account, Action<UnityUserData,T> call,T data)
        {
            if (mUserDataDict.ContainsKey(account))
            {
                UnityUserData userData = mUserDataDict[account];
                if (userData == null) return;
                call?.Invoke(userData, data);
                return;
            }
            IUnityUserDataCallBack callBack = new UserDataCallBack<T>(call, data);
            if (!mUserDataQueueCallBackDict.ContainsKey(account))
            {
                mUserDataQueueCallBackDict.Add(account, new Queue<IUnityUserDataCallBack>());
            }
            mUserDataQueueCallBackDict[account].Enqueue(callBack);
            SendGetUserDataRequest(account);
        }
        /// <summary>
        /// 映射用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="image"></param>
        /// <param name="name"></param>
        public static void MapUserData(int account, Action<UnityUserData> call)
        {
            if (mUserDataDict.ContainsKey(account))
            {
                UnityUserData userData = mUserDataDict[account];
                if (userData == null) return;
                call?.Invoke(userData);
                return;
            }
            IUnityUserDataCallBack callBack = new UserDataCallBack(call);
            if (!mUserDataQueueCallBackDict.ContainsKey(account))
            {
                mUserDataQueueCallBackDict.Add(account, new Queue<IUnityUserDataCallBack>());
            }
            mUserDataQueueCallBackDict[account].Enqueue(callBack);
            SendGetUserDataRequest(account);
        }
        /// <summary>
        /// 映射用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="image"></param>
        /// <param name="name"></param>
        public static void MapUserData(int account, Image image, Text name)
        {
            if (mUserDataDict.ContainsKey(account))
            {
                UnityUserData userData = mUserDataDict[account];
                if (userData == null) return;
                if (image != null) image.sprite = userData.headSprite;
                if (name != null) name.text = userData.userName;
                return;
            }
            IUnityUserDataCallBack callBack = new UserDataMap(image, name);
            if (!mUserDataQueueCallBackDict.ContainsKey(account))
            {
                mUserDataQueueCallBackDict.Add(account, new Queue<IUnityUserDataCallBack>());
            }
            mUserDataQueueCallBackDict[account].Enqueue(callBack);
            SendGetUserDataRequest(account);
        }
        /// <summary>
        /// 发送获取用户数据请求
        /// </summary>
        private static void SendGetUserDataRequest(int account)
        {
            //AppTools .UdpSend( SubServerType.Login,(short)LoginUdpCode.GetUserData, BitConverter.GetBytes(account));
        }
        /// <summary>
        /// 接收到了玩家的数据
        /// </summary>
        /// <param name="account"></param>
        /// <param name="userData"></param>
        public static void ReceiveUserDataCallBack(int account,UnityUserData userData) 
        {
            if (!mUserDataDict.ContainsKey(account)) 
            {
                mUserDataDict.Add(account,userData);
            }
            if (mUserDataQueueCallBackDict.ContainsKey(account))
            {
                Queue<IUnityUserDataCallBack> unityUserDataCallBacks = mUserDataQueueCallBackDict[account];
                while (unityUserDataCallBacks.Count!=0) {
                    IUnityUserDataCallBack unityUserDataCallBack = unityUserDataCallBacks.Dequeue();
                    unityUserDataCallBack.SetData(userData);
                }
            }
        }
    }
}
