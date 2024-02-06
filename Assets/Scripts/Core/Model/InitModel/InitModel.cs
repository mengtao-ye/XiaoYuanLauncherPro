using UnityEngine;
using YFramework;

namespace Game
{
    public class InitModel : BaseCustomTwoDModel
    {
        public InitModel(BaseScene scene, GameObject gameObject) : base(scene, gameObject)
        {

        }
        protected override void ConfigTwoDChildModel()
        {
            AddChildModel(new TwoDCameraChildModel(this,new GameObject("MainCamera")));
        }
    }
}
