using System;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class HotVersionTipUI : BaseCustomTipsUI
    {
        private Text mIntroduceText;
        private Action mSureAction;
        public HotVersionTipUI()
        {
        }
        public override void Awake()
        {
            mIntroduceText = transform.FindObject<Text>("IntroduceText");
            transform.FindObject<Button>("CancelBtn").onClick.AddListener(()=> {
                AppTools.QuitApp();
            });
            transform.FindObject<Button>("SureBtn").onClick.AddListener(() => {
                Hide();
                mSureAction?.Invoke();
            });
        }
        public void ShowContent(string introduce,Action sureAction)
        {
            mIntroduceText.text = introduce;
            mSureAction = sureAction;
        }
    }
}
