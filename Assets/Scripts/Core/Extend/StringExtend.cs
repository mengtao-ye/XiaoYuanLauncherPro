using YFramework;

namespace Game
{
    public static class StringExtend
    {
        /// <summary>
        /// 显示数据的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLen"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string LimitLen(this string str,int maxLen,string end = "...") {
            if (str == null) return string.Empty;
            if(maxLen<0) return string.Empty;
            if (maxLen >= str.Length)  return str;
            if(end == null) return string.Empty;
            return str.Substring(0, maxLen) + end;

        }
    }
}
