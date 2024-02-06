using System;
using UnityEngine;
using YFramework;

namespace Game
{
    public class UpdateLive : BaseLive
    {
        private float mRefreshTime;
        private float mTimer;
        public override LiveType liveType => LiveType.UpdateLive;
        public void SetData(float refreshTime,Action callBack,ILiveManager live) 
        {
            base.SetData(callBack, live);
            mRefreshTime = refreshTime;
            mTimer = 0;
        }

        public override void Update()
        {
            mTimer += Time.deltaTime;
            if (mTimer >= mRefreshTime)
            {
                mTimer = 0;
                if (mCallBack != null) mCallBack.Invoke();
            }
        }
        public override void Recycle()
        {
            ClassPool<UpdateLive>.Push(this) ;
        }
    }
}
