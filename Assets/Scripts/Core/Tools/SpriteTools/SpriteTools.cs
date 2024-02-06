using UnityEngine;

namespace Game
{
    public static partial class SpriteTools
    {
        /// <summary>
        /// 将byte数组转换成Sprite
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Sprite GetSprite(byte[] bytes,int width,int height)
        {
            Texture2D texture = new Texture2D(width, height);   // 先创建一个Texture2D对象，用于把流数据转成Texture2D
            texture.LoadImage(bytes);        // 流数据转换成Texture2D
            // 创建一个Sprite，以Texture2D对象为基础
            Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return sp;
        }
    }
}
