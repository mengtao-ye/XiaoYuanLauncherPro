using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class HttpManager : BaseModule ,IHttpManager
    {
        private Queue<IHttpRequest> mRequestQueue;
        public HttpManager(Center center) : base(center)
        {
            
        }
        public override void Awake()
        {
            mRequestQueue = new Queue<IHttpRequest>();
        }

        public void SendSpriteRequest(string url,Action<Sprite> callBack, Action<string> errorCallBack)
        {
            mRequestQueue.Enqueue(new SpriteHttpRequest(url,callBack, errorCallBack));
        }

        public void SendSpriteRequest<T>(string url, Action<Sprite, T> callBack, Action<string> errorCallBack, T value)
        {
            mRequestQueue.Enqueue(new SpriteHttpRequest<T>(url, callBack, errorCallBack, value));
        }
        public void AddRequest(IHttpRequest request)
        {
            mRequestQueue.Enqueue(request);
        }
        public override void Update()
        {
            if (mRequestQueue.Count != 0)
            {
                IHttpRequest request = mRequestQueue.Dequeue();
                if (request != null) request.Request();
            }
        }

    }
}
