namespace Game
{
    public static partial class AppTools
    {
        #region UI Operator
        /// <summary>
        /// 显示LogUI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T ShowLogUI<T>() where T : BaseCustomLogUI, new()
        {
            return GameCenter.Instance. curCanvas.logUIManager.ShowLogUI<T>();
        } 
        /// <summary>
        /// 显示提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T ShowTipsUI<T>() where T : BaseCustomTipsUI, new()
        {
            return GameCenter.Instance.curCanvas.showTipsPanel.ShowTipsUI<T>();
        }
        /// <summary>
        /// 显示提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void HideTipsUI<T>() where T : BaseCustomTipsUI, new()
        {
            GameCenter.Instance.curCanvas.showTipsPanel.HideTipsUI<T>();
        }
        /// <summary>
        /// 获取提示UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetTipsUI<T>() where T : BaseCustomTipsUI, new()
        {
            return GameCenter.Instance.curCanvas.showTipsPanel.GetTipsUI<T>();
        }
        /// <summary>
        /// 显示提示Panel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T ShowPanel<T>() where T : BaseCustomPanel, new()
        {
            return GameCenter.Instance.curCanvas.ShowPanel<T>();
        }
        /// <summary>
        /// 显示提示Panel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetPanel<T>() where T : BaseCustomPanel, new()
        {
            return GameCenter.Instance.curCanvas.FindPanel<T>();
        } 
        #endregion
        #region UI Toast
        /// <summary>
        /// 使用屏幕中方的Log打印信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void ToastSuccess<T>(T msg)
        {
            GameCenter.Instance.ShowLogUI<MidLogUI>().ShowContent(msg.ToString(), NotifyType.Success);
        }

        /// <summary>
        /// 使用屏幕中方的Log打印信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void ToastError<T>(T msg)
        {
            GameCenter.Instance.ShowLogUI<MidLogUI>().ShowContent(msg.ToString(), NotifyType.Error);
        }

        /// <summary>
        /// 使用屏幕中方的Log打印信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void ToastNotify<T>(T msg)
        {
            GameCenter.Instance.ShowLogUI<MidLogUI>().ShowContent(msg.ToString(), NotifyType.Notify);
        } 
        #endregion
    }
}
