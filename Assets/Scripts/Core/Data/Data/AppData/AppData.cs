namespace Game
{
    public static class AppData
    {
        public static PlatformType platformType = PlatformType.Test;
        public static ServerNetType netType = ServerNetType.Local;
        /// <summary>
        /// 当前运行的平台
        /// </summary>
        public static string RunPlatformName
        {
            get
            {
#if UNITY_ANDROID
                return "Android";
#elif UNITY_IOS
         return "iOS";
#endif
            }
        }
    }
    /// <summary>
    /// 平台环境
    /// </summary>
    public enum PlatformType
    {
        Test,//测试环境
        Pre,//预生产环境
        Pro//正式环境
    }

    public enum ServerNetType {
        Ali,
        Tencent,
        Local
    }

}
