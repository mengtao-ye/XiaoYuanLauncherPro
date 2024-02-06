using UnityEngine;

namespace Game
{
    public static class BuildData
    {
        public static string InstallApkBatDir = Application.dataPath + "/Editor/Bats";//自动执行安装包地址
        public static string InstallApkFileName = "InstallApk.bat";//自动执行安装包名称

        public const string KeystorePath = "D:/UnityData/user.keystore";//密钥路径
        public const string KeystorePassword = "528099";//密钥路径
        public const string keyaliasName = "YunCun";//密钥类型名称
        public const string keyaliasPassword = "528099";//密钥类型密码

        public const string ComponentName = "ZhouTao";//公司名称
        public const string ProductName = "YunCun";//项目名称
        public const string AppName = "云村";//项目名称
        public const string Version = "1.0.1";//项目版本   x.x.x 第一个x代表大版本，第二个代表迭代版本，第三个为修改Bug版本
        public static int VersionCode = GetVersionCode();//项目编号
        
        /// <summary>
        /// 根据版本号获取版本编码
        /// </summary>
        /// <returns></returns>
        private static int GetVersionCode() 
        {
            string[] strs = Version.Split('.');
            int bigVersion = int.Parse(strs[0]);
            int dieDaiVersion = int.Parse(strs[1]);
            int bugVersion = int.Parse(strs[2]);
            return int.Parse(bigVersion.ToString()+dieDaiVersion.ToString()+bugVersion.ToString());
        }
    }
}
