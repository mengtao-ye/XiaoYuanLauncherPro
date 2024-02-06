using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 游戏运行时加载数据面板
    /// </summary>
    public class InitPanel : BaseCustomPanel
    {
        private Text mContent;
        private Image mSlider;
        public InitPanel()
        {

        }
        public override void Start()
        {
            base.Start();
            mContent = transform.FindObject<Text>("LoadMsg");
            mSlider = transform.FindObject<Image>("LoadProcessValue");
            mSlider.fillAmount = 0;
        }
        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="process"></param>
        public void SetProcess(float process)
        {
            mSlider.fillAmount = process;
        }

        /// <summary>
        /// 设置加载过程中的内容
        /// </summary>
        /// <param name="content"></param>
        public void SetContent(string content)
        {
            mContent.text = content;    
        }
    }
}