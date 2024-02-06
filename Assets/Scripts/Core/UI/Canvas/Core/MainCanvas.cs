using YFramework;

namespace Game
{
    public class MainCanvas : BaseCustomCanvas
    {
        public MainCanvas(BaseScene scene, IMap<string, UIMapperData> map) : base(scene, map)
        {
        }
        public override void Awake()
        {
            base.Awake();
        }
    }
}
