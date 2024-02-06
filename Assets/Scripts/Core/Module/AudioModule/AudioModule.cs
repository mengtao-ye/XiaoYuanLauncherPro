using YFramework;

namespace Game
{
    /// <summary>
    /// 音效模块
    /// </summary>
    public static class AudioModule
    {
        /// <summary>
        /// 设置背景音
        /// </summary>
        /// <param name="b"></param>
        public static void SetBGAudioActive(bool b)
        {
            AudioPlayerModule.SetEnable(AudioType.BG, b);
        }
        /// <summary>
        /// 设置操作音
        /// </summary>
        /// <param name="b"></param>
        public static void SetOperatorAudioActive(bool b)
        {
            AudioPlayerModule.SetEnable(AudioType.Operator, b);
            AudioPlayerModule.SetEnable(AudioType.Chat, b);
            AudioPlayerModule.SetEnable(AudioType.Game, b);
            AudioPlayerModule.SetEnable(AudioType.Tips, b);
        }
    }
}
