using System;

namespace Game
{
    public abstract class BaseLive : ILive
    {
        protected Action mCallBack;
        private ILiveManager mLiveManager;
        public abstract LiveType liveType { get; }
        public bool isPop { get ; set ; }
        protected void  SetData(Action callBack,ILiveManager liveManager)
        {
            mCallBack = callBack;
            mLiveManager = liveManager;
        }

        public void Destory()
        {
            mLiveManager.RemoveLive(this);
        }
        public virtual void Update() { }
        public abstract void Recycle();
        public virtual void PopPool() { }
        public virtual void PushPool() { }
    }
}
