using UnityEngine;

namespace Game
{
    public class MainScene : BaseCustomScene
    {
        protected override string mSceneName =>SceneID.MainScene.ToString();
        public override void Awake()
        {
            canvas = new MainCanvas(this,UIMapper.Instance);
            model = new MainModel(this, new GameObject("_Model"));
            base.Awake();
        }
    }
}
