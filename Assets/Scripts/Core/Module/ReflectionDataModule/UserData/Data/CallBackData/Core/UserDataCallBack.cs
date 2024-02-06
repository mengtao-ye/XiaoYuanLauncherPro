using System;

namespace Game
{
    /// <summary>
    /// 用户数据回调
    /// </summary>
    public class UserDataCallBack : BaseUnityUserDataCallBack
    {
        private Action<UnityUserData> mCallBack;
        public UserDataCallBack(Action<UnityUserData> callback)
        {
            mCallBack = callback;
        }
        public override void SetData(UnityUserData userData)
        {
            if (userData != null)
            {
                mCallBack?.Invoke(userData);
            }
        }
    }

    /// <summary>
    /// 用户数据回调
    /// </summary>
    public class UserDataCallBack<T> : BaseUnityUserDataCallBack
    {
        private Action<UnityUserData, T> mCallBack;
        private T mData;
        public UserDataCallBack(Action<UnityUserData, T> callback, T data)
        {
            mCallBack = callback;
            mData = data;
        }
        public override void SetData(UnityUserData userData)
        {
            if (userData != null)
            {
                mCallBack?.Invoke(userData, mData);
            }
        }
    }
}
