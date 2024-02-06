using YFramework;

namespace Game
{
    public class InitCanvas : BaseCustomCanvas
    {
        public InitCanvas(BaseScene scene, IMap<string, UIMapperData> map) : base(scene, map)
        {
        }
        public override void Awake()
        {
            base.Awake();
            ShowPanel<InitPanel>();
        }
    }
}
