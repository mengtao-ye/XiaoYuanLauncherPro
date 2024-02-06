using UnityEngine;
using YFramework;

namespace Game
{
    public static class ConverterTools
    {
        /// <summary>
        /// 将Byte数组转换成Sprite对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Sprite BytesToSprite(byte[] data)
        {
            if (data.IsNullOrEmpty()) return null;
            Texture2D texture = new Texture2D(256 , 256 );
            texture.LoadImage(data);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
