using UnityEngine;

namespace Game
{
    public class SystemInfo
    {
        public const int DEFAULT_SCREEN_WIDTH = 750;//默认的宽
        public const int DEFAULT_SCREEN_HEIGHT = 1624;//默认的高
        public static float ScreenWidthRatio = Screen.width / (float)DEFAULT_SCREEN_WIDTH;
        public static float ScreenHeightRatio = Screen.height / (float)DEFAULT_SCREEN_HEIGHT;
    }
}
