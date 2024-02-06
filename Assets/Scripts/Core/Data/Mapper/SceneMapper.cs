using YFramework;

namespace Game
{
    public class SceneMapper: Map<string, IScene>
    {
        protected override void Config()
        {
            AddScene(new InitScene());
            AddScene(new LoginScene());
            AddScene(new MainScene());
        }
        /// <summary>
        /// 注册场景
        /// </summary>
        /// <param name="scene"></param>
        private void AddScene(IScene scene) {
            Add(scene.sceneName,scene);
        }
    }
}
