using YFramework;

namespace Game
{
    public abstract class BaseCustomCanvas : BaseCanvas
    {
        protected BaseCustomCanvas(BaseScene scene, IMap<string, UIMapperData> map) : base(scene, map)
        {
        }
    }
}
