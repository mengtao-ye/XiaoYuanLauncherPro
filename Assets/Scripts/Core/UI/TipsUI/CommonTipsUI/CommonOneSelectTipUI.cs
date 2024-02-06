using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 一个选择的弹窗界面
    /// </summary>
    public class CommonOneSelectTipUI : BaseCustomTipsUI
    {
        private Text mContent;
        private Text mOneSureText;
        private Text mTitle;
        private Action mSureAction;
        public CommonOneSelectTipUI()
        {

        }

        public override void Awake()
        {
            base.Awake();
            mTitle = transform.FindObject<Text>("Title");
            mContent = transform.FindObject<Text>("TextArea");
            mOneSureText = transform.FindObject<Text>("OneSureText");
            transform.FindObject<Button>("OneSureBtn").onClick.AddListener(() => {
                if (mSureAction != null)
                {
                    mSureAction.Invoke();
                }
            });
        }
        public void ShowContent(string content, string sureText, string title, Action sureAction)
        {
            mContent.text = content;
            mSureAction = sureAction;
            mOneSureText.text = sureText;
            mTitle.text = title;
        }
    }
}
