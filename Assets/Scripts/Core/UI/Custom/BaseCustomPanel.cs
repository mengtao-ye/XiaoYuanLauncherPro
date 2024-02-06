using UnityEngine.UI;
using YFramework;

namespace Game
{
    public abstract class BaseCustomPanel : BasePanel
    {
        public BaseCustomPanel()
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
            AudioPlayerModule.Play(AudioType.Operator, "Audios/Operator/Click");
        }
    }
}
