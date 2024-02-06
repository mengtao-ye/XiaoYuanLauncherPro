using UnityEngine;

namespace Game
{
    /// <summary>
    /// 路径数据
    /// </summary>
    public static class PathData
    {
        #region Panel根地址
        /// <summary>
        ///UIConstructionPanelParentPath根地址
        /// </summary>
        public const string UIStartPanelParentPath = UIPanelParentPath + "StartPanel/";
        /// <summary>
        ///UIConstructionPanelParentPath根地址
        /// </summary>
        public const string UIMainPanelParentPath = UIPanelParentPath + "Main/";
        /// <summary>
        /// 通用Panel根地址
        /// </summary>
        public const string UICommonPanelParentPath = UIPanelParentPath + "CommonPanel/";

        #endregion
        #region TipsUI 根地址
        /// <summary>
        /// 公共TipsUI根地址
        /// </summary>
        public const string UICommonTipsUIParentPath = UITipsParentPath + "CommonTipsUI/";
        #endregion
        #region UI 根地址
        /// <summary>
        /// UI Panel的根地址
        /// </summary>
        public const string UIPanelParentPath = "Prefabs/UI/Panel/";
        /// <summary>
        /// UI Log的根地址
        /// </summary>
        public const string UILogParentPath = "Prefabs/UI/Log/";
        /// <summary>
        /// UI TipsUI的根地址
        /// </summary>
        public const string UITipsParentPath = "Prefabs/UI/TipsUI/";
        #endregion
    }
}
