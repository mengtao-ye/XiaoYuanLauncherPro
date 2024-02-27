using System;
using YFramework;

namespace Game
{
    public class ResourcesLoadHelper : ResourceHelper
    {
        protected override void AsyncLoad<T>(string assetPath, Action<T> callBack)
        {
            T value = UnityEngine.Resources.Load<T>(assetPath);
            callBack?.Invoke(value);
        }

        protected override T Load<T>(string assetPath)
        {
            return UnityEngine.Resources.Load<T>(assetPath);
        }
    }
}
