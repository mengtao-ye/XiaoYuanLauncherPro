using UnityEngine;

namespace Game
{
    public class InitScene : BaseCustomScene
    {
        protected override string mSceneName => SceneID.InitScene.ToString();
        public override void Awake()
        {
            canvas = new InitCanvas(this,UIMapper.Instance);
            model = new InitModel(this, new GameObject("_Model"));
            controller = new InitController(this);
            base.Awake();
        }
        
    }
}
