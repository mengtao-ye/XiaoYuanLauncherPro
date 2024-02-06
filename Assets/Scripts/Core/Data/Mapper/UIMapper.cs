using YFramework;

namespace Game
{
    /// <summary>
    /// 公共的UI配置文件
    /// </summary>
    public class UIMapper : SingleMap<string, UIMapperData, UIMapper>
    {
        /// <summary>
        /// 面板父地址
        /// </summary>
        public const string PANEL_PARENT_PATH = "UI/Panel/";
        /// <summary>
        /// 提示内容父地址
        /// </summary>
        public const string TIP_UI_PARENT_PATH = "UI/TipUI/";
        /// <summary>
        /// 通用提示内容地址
        /// </summary>
        public const string TIP_UI_COMMON_PARENT_PATH = TIP_UI_PARENT_PATH+ "CommonTipsUI/";

        protected override void Config()
        {
            AddUI<InitPanel>(PANEL_PARENT_PATH);
            AddUI<CommonTwoSelectTipUI>(TIP_UI_COMMON_PARENT_PATH);
            AddUI<CommonOneSelectTipUI>(TIP_UI_COMMON_PARENT_PATH);
        }
        /// <summary>
        /// 注册面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentPath"></param>
        protected void AddUI<T>(string parentPath) where T : IUI
        {
            Add(typeof(T).Name, new UIMapperData(parentPath + typeof(T).Name));

        }
    }
}
