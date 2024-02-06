using UnityEngine;

namespace Game
{
    public class LoginScene : BaseCustomScene
    {
        protected override string mSceneName => SceneID.LoginScene.ToString();
        public override void Awake()
        {
            canvas = new LoginCanvas(this,UIMapper.Instance);
            model = new LoginModel(this, new GameObject("_Model"));
            base.Awake();
        }
    }
}
