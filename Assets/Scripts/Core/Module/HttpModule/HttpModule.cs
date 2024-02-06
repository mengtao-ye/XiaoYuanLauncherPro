using System;
using UnityEngine.Networking;
using YFramework;

public static class HttpModule
{
    /// <summary>
    /// ÇëÇóÊý¾Ý
    /// </summary>
    /// <param name="url"></param>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    public static void Get(string url, Action<byte[]> success, Action fail)
    {
        IEnumeratorModule.StartCoroutine(IEGet(url,success,fail));    
    }

    private static System.Collections.IEnumerator IEGet(string url,  Action<byte[]> success, Action fail)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            fail?.Invoke();
        }
        else {
            success?.Invoke(request.downloadHandler.data);
        }
    }
}
