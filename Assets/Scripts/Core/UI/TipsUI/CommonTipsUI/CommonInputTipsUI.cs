using System;
using UnityEngine.UI;
using YFramework;
using static UnityEngine.UI.InputField;

namespace Game
{
    public class CommonInputTipsUI : BaseCustomTipsUI
    {
        private Text mTitle;
        private InputField mIF;
        private Text mSureText;
        private Action<string> mSureAction;
        public CommonInputTipsUI()
        {

        }

        public override void Awake()
        {
            base.Awake();
            mTitle = transform.FindObject<Text>("Title");
            mIF = transform.FindObject<InputField>("InputField");
            mSureText = transform.FindObject<Text>("SureText");

            transform.FindObject<Button>("SureBtn").onClick.AddListener(SureBtnListener);
            transform.FindObject<Button>("CloseBtn").onClick.AddListener(CloseBtnListener);
        }

        private void SureBtnListener()
        {
            mSureAction?.Invoke(mIF.text);
            Hide();
            mIF.text = "";
        }
        private void CloseBtnListener()
        {
            Hide();
        }
        public void ShowContent(string title, Action<string> sureCallBack, string sureText, ContentType contentType, int characterLimit,string defaultValue = "")
        {
            mSureAction = sureCallBack;
            mTitle.text = title;
            mSureText.text = sureText;
            mIF.contentType = contentType;
            mIF.characterLimit = characterLimit;
            mIF.text = defaultValue;
        }
    }
}
