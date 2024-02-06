using System;
using YFramework;

namespace Game
{
    /// <summary>
    /// 生命周期管理器接口
    /// </summary>
    public interface ILiveManager : IModule
    {
        ILive AddUpdate(float refreshTime, Action callBack);
        void RemoveLive(ILive live) ;
    }
}
