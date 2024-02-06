using System;
using UnityEngine;
using YFramework;

namespace Game
{
    public interface IHttpManager : IModule
    {
        /// <summary>
        /// 发送Sprite下载请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callBack"></param>
        /// <param name="errorCallBack"></param>
        void SendSpriteRequest(string url, Action<Sprite> callBack, Action<string> errorCallBack);
        /// <summary>
        /// 发送Sprite下载请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callBack"></param>
        /// <param name="errorCallBack"></param>
        void SendSpriteRequest<T>(string url, Action<Sprite,T> callBack, Action<string> errorCallBack,T value);
    }
}
