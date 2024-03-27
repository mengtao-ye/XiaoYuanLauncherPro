using YFramework;

namespace Game
{
    /// <summary>
    /// 拼音转换工具
    /// </summary>
    public static class PinYinTools
    {
        /// <summary>
        /// 其他默认字符
        /// </summary>
        private const char OTHER_CODE = '{';
        /// <summary>
        /// 获取汉字首字母 
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>汉字首字母，不是汉字的话返回#</returns>
        public static char GetHanZiFirstCode(string ch)
        {
            if (ch.IsNullOrEmpty()) return OTHER_CODE;
            return GetHanZiFirstCode(ch[0]);
        }
        /// <summary>
        /// 获取汉字首字母 
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>汉字首字母，不是汉字的话返回#</returns>
        public static char GetHanZiFirstCode(char ch)
        {
            string str = NPinyin.Pinyin.GetPinyin(ch);
            if (ch.ToString() == str)
            {
                //不是汉字
                return OTHER_CODE;
            }
            if (str.IsNullOrEmpty())
            {
                //转换失败
                return OTHER_CODE;
            }
            return str[0];
        }
    }
}
