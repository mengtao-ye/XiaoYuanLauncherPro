using System;
using UnityEngine;
using static YFramework.Utility;

namespace Game
{
    public class SpriteHttpRequest : BaseHttpRequest
    {
        private Action<Sprite> mCallBack;
        public SpriteHttpRequest(string url, Action<Sprite> callBack, Action<string> errorCallBack) : base(url, errorCallBack)
        {
            mCallBack = callBack;
        }

        protected override void CallBack(byte[] data)
        {
            if (mCallBack != null)
            {
                mCallBack(ConverterTools.BytesToSprite(data));
            }
        }
    }
}
