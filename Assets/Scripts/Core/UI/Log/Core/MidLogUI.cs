using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public enum NotifyType
    {
        Notify,
        Error,
        Success
    }
    public class MidLogUI : BaseCustomLogUI
    {
        private Text mContent;
        private Coroutine mAudoCloseCor;
        private Color mTrueColor;
        private Color mErrorColor;
        private Color mNotifyColor;
        public MidLogUI()
        {

        }
        public override void Awake()
        {
            mTrueColor = new Color(1, 1, 1);
            mErrorColor = new Color(0.9528302f, 0.326814f, 0.0665183f);
            mNotifyColor = new Color(0.8666667f, 1f, 0.1098039f);
            mAudoCloseCor = null;
            mContent = transform.FindObject<Text>("Content");
        }
        public void ShowContent(string content, NotifyType type)
        {
            if (mAudoCloseCor != null)
            {
                IEnumeratorModule.StopCoroutine(mAudoCloseCor);
            }
            mContent.text = content;
            mAudoCloseCor = IEnumeratorModule.StartCoroutine(IEAudoClose());
            switch (type)
            {
                case NotifyType.Notify:
                    mContent.color = mNotifyColor;
                    break;
                case NotifyType.Error:
                    mContent.color = mErrorColor;
                    break;
                case NotifyType.Success:
                    mContent.color = mTrueColor;
                    break;
            }
        }
        private IEnumerator IEAudoClose()
        {
            yield return Yielders.GetSeconds(2) ;
            Hide();
        }
    }
}
