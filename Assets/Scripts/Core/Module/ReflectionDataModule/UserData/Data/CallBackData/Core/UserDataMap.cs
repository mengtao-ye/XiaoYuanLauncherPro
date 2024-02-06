using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 用户数据映射
    /// </summary>
    public class UserDataMap: BaseUnityUserDataCallBack
    {
        private Image mHead;
        private Text mNameText;
        public UserDataMap(Image head,Text nameText)
        {
            mHead = head;
            mNameText = nameText;
        }
        public override void SetData(UnityUserData userData)
        {
            if (mHead != null) mHead.sprite = userData.headSprite;
            if (mNameText != null) mNameText.text = userData.userName;
        }
    }
}
