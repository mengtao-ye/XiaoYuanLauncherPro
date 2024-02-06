using UnityEngine;
using YFramework;

namespace Game
{
    class MyResources : Resource
    {
        protected override T Load<T>(string assetPath)
        {
            int index = assetPath.IndexOf("Resources/");
            if (index != -1)
            {
                //传递的是全路径地址
                index += 10;
                int endIndex = assetPath.LastIndexOf('.');
                assetPath = assetPath.Substring(index, endIndex - index);
            }
            return UnityEngine.Resources.Load<T>(assetPath);
        }
    }
}
