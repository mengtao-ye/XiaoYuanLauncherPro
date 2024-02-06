using UnityEngine;

namespace Game
{
    public static class BatTools
    {
        /// <summary>
        /// 运行bat代码
        /// </summary>
        /// <param name="batFile"></param>
        /// <param name="workingDir"></param>
        public static void RunMyBat(string batFile, string workingDir)
        {
            if (!System.IO.Directory.Exists(workingDir))
            {
                Debug.LogError("bat文件不存在：" + workingDir);
            }
            else
            {
                EdtUtil.RunBat(batFile, "", workingDir);
            }
        }
        
    }
}
