using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Unity平台用户信息
    /// </summary>
    public  class UnityUserData
    {
        public int UserID;
        public int account;
        public string userName;
        public string headUrl;
        public Sprite headSprite;
        public UnityUserData(int userID,int account,string userName,string headUrl,Sprite headSprite)
        {
            this.UserID = userID;
            this.account = account;
            this.userName = userName;
            this.headUrl = headUrl;
            this.headSprite = headSprite;
        }
    }
}
