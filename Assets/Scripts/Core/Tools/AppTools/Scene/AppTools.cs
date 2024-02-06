using System;

namespace Game
{
    public static partial class AppTools
    {
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadPorcess"></param>
        public static void LoadScene(SceneID sceneName, Action<float> loadPorcess = null) {
            GameCenter.Instance.LoadScene(sceneName, loadPorcess);
        }
    }
}
