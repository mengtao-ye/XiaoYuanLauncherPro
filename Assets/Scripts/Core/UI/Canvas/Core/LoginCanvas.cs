using UnityEngine;
using YFramework;

namespace Game
{
    public class LoginCanvas : BaseCustomCanvas
    {
        public LoginCanvas(BaseScene scene, IMap<string, UIMapperData> map) : base(scene, map)
        {
        }
        public override void Awake()
        {
            base.Awake();
           Debug.Log( GameCenter.Instance.UnityToAndroid(1,1,1,1,"","","'"));
        }
    }
}
