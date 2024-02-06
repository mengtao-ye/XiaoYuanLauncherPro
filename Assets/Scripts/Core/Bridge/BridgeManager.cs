using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// unity与原生交互类
    /// </summary>
    public class BridgeManager : BaseModule
    {
        private AndroidJavaObject mAndroidBridge;
        public BridgeManager(Center center) : base(center)
        {

        }
        public override void Awake()
        {
            base.Awake();
#if UNITY_ANDROID && !UNITY_EDITOR
            InitAndroidBridge();
#endif
            isRun = true;
        }
        private void InitAndroidBridge()
        {
            try
            {
                mAndroidBridge = new AndroidJavaObject("com.ZhouTao.XiaoYuan.MainActivity");
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                isRun = false;
            }
        }
        #region UnityToAndroid
#if UNITY_ANDROID && !UNITY_EDITOR
        public string UnityToAndroid(int id,  int value1,int value2,int value3,string str1,string str2,string str3)
        {
            if (mAndroidBridge==null) 
            {
                Debug.LogError("AndroidBridge is null");
                return null;
            }
            return mAndroidBridge.Call<string>("UnityToAndroid", id, value1,value2,value3,str1,str2,str3);
        }
#else
        public string UnityToAndroid(int id, int value1, int value2, int value3, string str1, string str2, string str3)
        {
            Debug.Log("Editor模式下无法调用");
            return null;
        }
#endif
        #endregion

    }
}
