using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 加载app原始数据
    /// </summary>
    public class LoadMainFileChildController : BaseCustomChildController
    {
        public InitPanel initPanel { get; private set; }
        public LoadMainFileChildController(BaseController controller) : base(controller)
        {

        }
        public override void Start()
        {
            initPanel = controller.scene.canvas.FindPanel<InitPanel>();
            IProcess process =   GameCenter.Instance.processController.Create<LoadOssCondition>()
               .Concat(new LoadHotVersionProcess(this))
                .Concat(new LoadHotIntroduceProcess(this))
                .Concat(new LoadHotConfigProcess(this))
                .Concat(new LoadHotFileProcess(this))
                .Concat(new LoadOriginalVersionProcess(this))
                .Concat(new LoadConfigProcess(this))
                .Concat(new LoadFileProcess(this))
                ;
            process.processManager.Launcher();
        }
        /// <summary>
        /// AB包资源都加载完成了
        /// </summary>
        public void ABAssetLoadSuccess()
        {
            AssetBundleModule.Add(ABTag.Main);
            LoadMainLauncher();
            LoadLauncherBridge();
            AppTools.LoadScene( SceneID.LoginScene);
        }

        /// <summary>
        /// 加载与启动器之间的桥梁对象
        /// </summary>
        private void LoadLauncherBridge()
        {
            GameObject mainBridge = ResourceModule.Load<GameObject>("LauncherBridge", ABTag.Main).InstantiateGameObject();
            mainBridge.DontDestroyOnLoad();
            GameCenter.Instance.packageBridgeManaegr.Add(new BridgeData() {  tagName = ABTag.Main, target  = mainBridge });
            mainBridge.SendMessage("Init", GameCenter.Instance.packageBridgeManaegr.bridgeGo, SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 加载主程序的启动器
        /// </summary>
        private void LoadMainLauncher()
        {
            GameObject mainBridge = ResourceModule.Load<GameObject>("MainLauncher", ABTag.Main).InstantiateGameObject();
            mainBridge.DontDestroyOnLoad();
        }
    }
}
