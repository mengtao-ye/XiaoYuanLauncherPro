using UnityEngine;

namespace Game
{
    public class XiaoYuanYFrameworkHelper : YFramework.YFrameworkHelper
    {
        public override bool IsInit { get; set; } = true;
        public override Vector2 ScreenSize { get; set; } = new Vector2(1080,1920);
    }
}
