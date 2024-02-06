namespace Game
{
    public static class AppConstData
    {
#if UNITY_EDITOR
        public const bool UseABLoad = true;//是否使用AB包加载
#else
        public const bool UseABLoad = false;//是否使用AB包加载
#endif
    }
}
