using UnityEngine;
using YFramework;

namespace Game
{
    public class TwoDCameraChildModel : BaseCustomChildModel
    {
        /// <summary>
        /// 主相机
        /// </summary>
        private Camera mMainCamera;
        public Camera mainCamera { get { return mMainCamera; } }
        public TwoDCameraChildModel(BaseModel model, GameObject target) : base(model, target)
        {
        }
        public override void Awake()
        {
            base.Awake();
            gameObject.AddComponent<AudioListener>();
            mMainCamera = gameObject.AddComponent<Camera>();
        }
    }
}
