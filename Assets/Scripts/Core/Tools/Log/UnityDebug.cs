using YFramework;

namespace Game
{
    /// <summary>
    /// Unity 内置打印消息
    /// </summary>
    public class UnityDebug : Log
    {
        protected override void LogErrorMsg<T>(T msg)
        {
            UnityEngine.Debug.LogError(msg.ToString());
        }

        protected override void LogMsg<T>(T msg)
        {
            UnityEngine.Debug.Log(msg.ToString());
        }

        protected override void LogWarningMsg<T>(T msg)
        {
            UnityEngine.Debug.LogWarning(msg.ToString());
        }
    }
}
