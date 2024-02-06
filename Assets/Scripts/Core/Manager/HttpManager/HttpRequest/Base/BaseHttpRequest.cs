using System;
using UnityEngine.Networking;
using YFramework;

namespace Game
{
    public abstract class BaseHttpRequest : IHttpRequest
    {
        private string mUrl;//请求地址
        private Action<string> mErrorCallBack;//请求回调
        public BaseHttpRequest(string url, Action<string> errorCallBack)
        {
            mUrl = url;
            mErrorCallBack = errorCallBack;
        }
        protected abstract void CallBack(byte[] data);
        public void Request()
        {
            if (mUrl.IsNullOrEmpty())
            {
                Log.LogError("请求地址不能为空");
                return;
            }
            IEnumeratorModule.StartCoroutine(IESendWebRequest());
        }
        private System.Collections.IEnumerator IESendWebRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get(mUrl);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                if (mErrorCallBack != null) mErrorCallBack.Invoke(request.error);
            }
            else if (!request.isDone)
            {
                if (mErrorCallBack != null) mErrorCallBack.Invoke("下载失败");
            }
            else 
            {
                CallBack( request.downloadHandler.data);
            }
        }
    }
}
