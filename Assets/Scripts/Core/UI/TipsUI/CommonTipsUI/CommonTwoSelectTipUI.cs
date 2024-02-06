using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 两个弹窗的提示内容
    /// </summary>
    public class CommonTwoSelectTipUI : BaseCustomTipsUI
    {
        private Text mContent;
        private Action mSureAction;
        private Action mCancelAction;
        private Text mTwoSureText;
        private Text mTwoCancelText;
        private Text mTitle;
        public CommonTwoSelectTipUI()
        {

        }

        public override void Awake()
        {
            base.Awake();
            mTitle = transform.FindObject<Text>("Title");
            mContent = transform.FindObject<Text>("TextArea");
            mTwoSureText = transform.FindObject<Text>("TwoSureText");
            mTwoCancelText = transform.FindObject<Text>("TwoCancelText");
            transform.FindObject<Button>("TwoSureBtn").onClick.AddListener(() => {
                if (mSureAction != null)
                {
                    mSureAction.Invoke();
                }
            });
            transform.FindObject<Button>("TwoCancelBtn").onClick.AddListener(() => {
                if (mCancelAction != null)
                {
                    mCancelAction.Invoke();
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">显示的内容</param>
        /// <param name="title">标题</param>
        /// <param name="cancelText">取消按钮文字提示</param>
        /// <param name="sureText">确认内容文字提示</param>
        /// <param name="sureAction">确认回调</param>
        /// <param name="cancelAction">取消回调，自带点击关闭回调</param>
        public void ShowContent(string content,  string title, string cancelText, string sureText, Action sureAction, Action cancelAction )
        {
            mContent.text = content;
            mSureAction = sureAction;
            mCancelAction = ()=> 
            { 
                Hide();
                cancelAction?.Invoke();
            };
            mTwoSureText.text = sureText;
            mTwoCancelText.text = cancelText;
            mTitle.text = title;
        }
    }
}
