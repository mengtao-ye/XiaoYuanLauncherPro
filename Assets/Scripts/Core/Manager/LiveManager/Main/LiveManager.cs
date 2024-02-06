using System;
using System.Collections.Generic;
using YFramework;

namespace Game
{
    public class LiveManager : BaseModule, ILiveManager
    {
        private List<ILive> mLiveList = new List<ILive>();
        public LiveManager(Center center) : base(center)
        {
        }
        public override void Update()
        {
            for (int i = 0; i < mLiveList.Count; i++)
            {
                mLiveList[i].Update();
            }
        }
        /// <summary>
        /// 添加更新回调
        /// </summary>
        /// <param name="refreshTime"></param>
        /// <param name="callBack"></param>
        public ILive AddUpdate(float refreshTime,Action callBack)
        {
            UpdateLive lift = ClassPool<UpdateLive>.Pop(refreshTime, callBack, this);
            mLiveList.Add(lift);
            return lift;
        }
        /// <summary>
        /// 移除生命周期对象
        /// </summary>
        /// <param name="live"></param>
        public void RemoveLive(ILive live)
        {
            if (live == null) return;
            if (mLiveList.Contains(live))
            {
                mLiveList.Remove(live);
                live.Recycle();
            }
        }
    }
}
