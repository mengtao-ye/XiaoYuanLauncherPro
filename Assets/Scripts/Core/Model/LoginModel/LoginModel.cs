using UnityEngine;
using YFramework;

namespace Game
{
    public class LoginModel : BaseCustomTwoDModel
    {
        public LoginModel(BaseScene scene, GameObject gameObject) : base(scene, gameObject)
        {

        }
        protected override void ConfigTwoDChildModel()
        {
            AddChildModel(new TwoDCameraChildModel(this, new GameObject("MainCamera")));
        }
    }
}
