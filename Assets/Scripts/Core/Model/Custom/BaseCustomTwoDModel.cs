using UnityEngine;
using YFramework;

namespace Game
{
    public abstract class BaseCustomTwoDModel : BaseCustomModel
    {
        protected BaseCustomTwoDModel(BaseScene scene, GameObject gameObject) : base(scene, gameObject)
        {
        }
        protected override void ConfigChildModel()
        {
            ConfigTwoDChildModel();
        }
        protected abstract void ConfigTwoDChildModel();
    }
}
