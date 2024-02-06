using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 3D场景Model管理器
    /// </summary>
    public abstract class BaseCustomThreeDModel : BaseCustomModel
    {
        public BaseCustomThreeDModel(BaseScene scene, GameObject gameObject) : base(scene, gameObject)
        {

        }
        protected override void ConfigChildModel()
        {
            ConfigThreeDChildModel();
        }
        protected abstract void ConfigThreeDChildModel();
    }
}
