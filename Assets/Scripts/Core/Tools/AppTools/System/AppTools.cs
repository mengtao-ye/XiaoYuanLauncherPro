using YFramework;

namespace Game
{
    public static partial class AppTools
    {
        /// <summary>
        /// 退出应用
        /// </summary>s
        public static void QuitApp()
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        /// <summary>
        /// 根据版本号获取版本编码
        /// </summary>
        /// <returns></returns>
        public static int GetVersionCode(string version)
        {
            try
            {
                string[] strs = version.Split('.');
                int bigVersion = int.Parse(strs[0]);
                int dieDaiVersion = int.Parse(strs[1]);
                int bugVersion = int.Parse(strs[2]);
                return int.Parse(bigVersion.ToString() + dieDaiVersion.ToString() + bugVersion.ToString());
            }
            catch
            {
                LogHelper.LogError("版本异常：" + version);
                return 0;
            }
        }
    }
}
