using YFramework;

namespace Game
{
    public interface ILive : IPool
    {
        LiveType liveType { get; }
        void Update();
        void Destory();
    }
}
