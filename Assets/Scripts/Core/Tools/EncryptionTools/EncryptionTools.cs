using System;
using YFramework;

namespace Game
{
    /// <summary>
    /// 加密工具
    /// </summary>
    public static  class EncryptionTools
    {
        /// <summary>
        /// 加密码
        /// </summary>
        private static byte[] EncryptionCode = new byte[] { 1, 21, 52, 234, 76, 89, 4, 211, 145, 167, 34, 156, 78, 91, 90, 12, 198, 234, 254, 122, 111, 16, 75, 35, 79, 96, 72, 36, 254, 156, 90, 14 };
        /// <summary>
        /// 获取加密后的字节
        /// </summary>
        /// <param name="targetCode"></param>
        /// <param name="encryptionCode"></param>
        /// <returns></returns>
        private static byte GetEncryptionCode(byte targetCode,byte encryptionCode) {
           return (byte)((targetCode + encryptionCode) % (byte.MaxValue + 1));//加密后的数据
        }

        /// <summary>
        /// 获取加密后的字节
        /// </summary>
        /// <param name="targetCode"></param>
        /// <param name="encryptionCode"></param>
        /// <returns></returns>
        private static byte GetDecryptionCode(byte targetCode, byte encryptionCode)
        {
            return (byte)((targetCode - encryptionCode) % (byte.MaxValue + 1));//解密数据
        }

        /// <summary>
        /// 数据加密
        /// </summary>
        /// <param name="data"></param>
        public static void Encryption(byte[] data) 
        {
            if (data.IsNullOrEmpty()) return;
            int min = Math.Min(data.Length, EncryptionCode.Length);
            for (int i = 0; i < min; i++)
            {
                data[i] = GetEncryptionCode(data[i], EncryptionCode[i]);
            }
        }

        /// <summary>
        /// 数据全部数据
        /// </summary>
        /// <param name="data"></param>
        public static void EncryptionAll (byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = GetEncryptionCode(data[i], EncryptionCode[i % EncryptionCode.Length]);
            }
        }
        /// <summary>
        /// 数据解密
        /// </summary>
        /// <param name="data"></param>
        public static void Decryption(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            int min = Math.Min(data.Length, EncryptionCode.Length);
            for (int i = 0; i < min; i++)
            {
                data[i] = GetDecryptionCode(data[i], EncryptionCode[i]);
            }
        }
        /// <summary>
        /// 数据全部解密
        /// </summary>
        /// <param name="data"></param>
        public static void DecryptionAll(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = GetDecryptionCode(data[i], EncryptionCode[i % EncryptionCode.Length]);
            }
        }
    }
}
