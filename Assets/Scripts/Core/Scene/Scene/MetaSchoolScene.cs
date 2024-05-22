using UnityEngine;

namespace Game
{
    public class MetaSchoolScene : BaseCustomScene
    {
        protected override string mSceneName =>SceneID.MetaSchoolScene.ToString();
        public override void Awake()
        {
            canvas = new MetaSchoolCanvas(this,UIMapper.Instance);
            model = new MetaSchoolModel(this, new GameObject("_Model"));
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
            GameCenter.Instance.packageBridgeManaegr.ChangeScene(ABTag.Main); 
        }
    }
}
