using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 带参数的请求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpriteHttpRequest<T> : BaseHttpRequest
    {
        private Action<Sprite,T> mCallBack;
        private T mValue;
        public SpriteHttpRequest(string url, Action<Sprite,T> callBack, Action<string> errorCallBack,T value) : base(url, errorCallBack)
        {
            mCallBack = callBack;
            mValue = value;
        }

        protected override void CallBack(byte[] data)
        {
            if (mCallBack != null)
            {
                mCallBack(ConverterTools.BytesToSprite(data), mValue);
            }
        }
    }
}
