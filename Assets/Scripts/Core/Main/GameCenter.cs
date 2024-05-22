using System;
using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 项目核心处理器
    /// </summary>
    public class GameCenter : SingletonMono<GameCenter>
    {
        private static bool mRun = true; //是否运行YFramwork框架
        #region Field
        public Center center { get; private set; } = null;
        private XiaoYuanSceneManager mSceneManager;
        //private BridgeManager mBridgeManager;
        public IScene curScene { get { return mSceneManager.curScene; } }
        public ICanvas curCanvas { get { return mSceneManager.curScene.canvas; } }
        public IModel curModel { get { return mSceneManager.curScene.model; } }
        public IController curController { get { return mSceneManager.curScene.controller; } }
        public ProcessController processController { get; private set; }
        public PackageBridgeManager packageBridgeManaegr { get; private set; }
        #endregion
        #region Init
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            if (!mRun) return;
            UnityEngine.Debug.Log("开始运行YFramework");
            GameObject yCenter = new GameObject("YFramework");
            yCenter.AddComponent<GameCenter>();
            DontDestroyOnLoad(yCenter);
            yCenter.hideFlags = HideFlags.HideAndDontSave;
        }
        /// <summary>
        /// 最先执行的方法
        /// </summary>
        private void InitData()
        {
            Instance = this;
            #region  设置屏幕旋转
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = true;
            #endregion
            #region UnityEditorSetting
#if UNITY_EDITOR
            Application.targetFrameRate = 500;//设置最高帧率
#else
            Application.targetFrameRate = 60;//设置最高帧率
#endif
            #endregion
            YFrameworkHelper.Instance = new XiaoYuanYFrameworkHelper();
            TotweenModule.Init();
            LogHelper.Instance = new UnityLogHelper();
            ResourceHelper.Instance = new ResourcesLoadHelper();
        }
        private void Awake()
        {
            InitData();
            processController = new ProcessController();
            center = new Center();
            mSceneManager = new XiaoYuanSceneManager(center, new SceneMapper());
            //mBridgeManager = new BridgeManager(center);
            packageBridgeManaegr = new PackageBridgeManager(center);
            ConfigSceneManager();
            center.AddGame(packageBridgeManaegr);
            //center.AddGame(mBridgeManager);
            center.AddGame(mSceneManager);
            center.Awake();
            
        }
        private void Start()
        {
            center.Start();
        }
        private void Update()
        {
            center.Update();
            processController.Update();
        }
        public void LateUpdate()
        {
            center.LaterUpdate();
        }
        private void FixedUpdate()
        {
            center.FixedUpdate();
        }
        #endregion

        #region UI
        /// <summary>
        /// 显示LogUI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ShowLogUI<T>(Action<T> action = null) where T : BaseCustomLogUI, new()
        {
             curCanvas.logUIManager.ShowLogUI<T>(action);
        }
        /// <summary>
        /// 显示提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void  ShowTipsUI<T>(Action<T> action = null) where T : BaseCustomTipsUI, new()
        {
             curCanvas.showTipsPanel.ShowTipsUI<T>(action);
        }
        /// <summary>
        /// 显示提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void HideTipsUI<T>() where T : BaseCustomTipsUI, new()
        {
            curCanvas.showTipsPanel.HideTipsUI<T>();
        }
        /// <summary>
        /// 获取提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void GetTipsUI<T>(Action<T> action = null) where T : BaseCustomTipsUI, new()
        {
             curCanvas.showTipsPanel.GetTipsUI<T>(action);
        }
        /// <summary>
        /// 显示提示Panel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ShowPanel<T>(Action<T> action = null) where T : BaseCustomPanel, new()
        {
             curCanvas.ShowPanel<T>(action);
        }
        /// <summary>
        /// 显示提示Panel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetPanel<T>() where T : BaseCustomPanel, new()
        {
            return curCanvas.FindPanel<T>();
        }
        #endregion
        #region SceneManager

        /// <summary>
        /// 配置场景加载数据
        /// </summary>
        private void ConfigSceneManager()
        {
            mSceneManager.SetLoadCompleteCallBack(SceneLoadComplete);
        }
        /// <summary>
        /// 场景加载完成的回调
        /// </summary>
        /// <param name="sceneName"></param>
        private void SceneLoadComplete(string sceneName)
        {
            GameObjectPoolModule.Clear();
            IEnumeratorModule.StopAllCoroutine();//停止所有的携程
        }
        /// <summary>
        /// 场景加载
        /// </summary>
        /// <param name="sceneName">加载的名称</param>
        /// <param name="loadPorcess">加载的进度</param>
        public void LoadScene(SceneID sceneName, Action<float> loadPorcess = null)
        {
            mSceneManager.LoadScene(sceneName.ToString(), loadPorcess);
        }
        /// <summary>
        /// 场景加载
        /// </summary>
        /// <param name="sceneName">加载的名称</param>
        /// <param name="loadPorcess">加载的进度</param>
        public void LoadScene(string sceneName, Action<float> loadPorcess = null)
        {
            mSceneManager.LoadScene(sceneName, loadPorcess);
        }
        #endregion
        #region 原生之间调用
        //public string UnityToAndroid(int id, int value1, int value2, int value3, string str1, string str2, string str3)
        //{
        //    if (mBridgeManager.isRun)
        //    {
        //        return mBridgeManager.UnityToAndroid(id, value1, value2, value3, str1, str2, str3);
        //    }
        //    return null;
        //}
        #endregion
    }
}
