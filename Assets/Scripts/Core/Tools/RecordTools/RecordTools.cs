using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 记录信息到本地
    /// </summary>
    public static class RecordTools
    {
        private static List<string> mErrorList = new System.Collections.Generic.List<string>();
        /// <summary>
        /// 记录错误到本地
        /// </summary>
        public static void AddError(string content)
        {
            if (string.IsNullOrEmpty(content)) return;
            if (mErrorList.Contains(content)) return;//相同的错误不用记录
            mErrorList.Add(content) ;
            FileTools.CreateFile(RecordData.RECORD_PATH);
            if (content.Length > 1000)
            {
                content = content.Substring(0,1000);
            }
            File.AppendAllText(RecordData.RECORD_PATH, "@" + content);
        }
        /// <summary>
        /// 获取本地的错误信息
        /// </summary>
        public static string[] GetError()
        {
            if (!File.Exists(RecordData.RECORD_PATH)) return null;
            return File.ReadAllText(RecordData.RECORD_PATH).Split('@');
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public static void Clear() 
        {
            FileTools.ClearTxt(RecordData.RECORD_PATH);
        }
    }
}
