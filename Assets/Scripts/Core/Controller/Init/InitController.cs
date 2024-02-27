using YFramework;

namespace Game
{
    public class InitController : BaseCustomController
    {
        public InitController(BaseScene scene) : base(scene)
        {

        }

        protected override void ConfigChildController()
        {
            AddChildController(new LoadMainFileChildController(this));
        }
    }
}
