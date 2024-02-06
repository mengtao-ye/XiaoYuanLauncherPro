using UnityEngine.UI;
using YFramework;

namespace Game
{
    public abstract class BaseCustomTipsUI : BaseTipsUI
    {
        public BaseCustomTipsUI()
        {

        }
        public override void Awake()
        {
            Button[] btns = transform.GetComponentsInChildren<Button>();
            for (int i = 0; i < btns.Length; i++)
            {
                btns[i].onClick.AddListener(ClickAudio);
            }
        }
        private void ClickAudio()
        {
            YFramework.AudioPlayerModule.Play(YFramework.AudioType.Operator, "Audios/Operator/Click_Audio");
        }
    }
}
