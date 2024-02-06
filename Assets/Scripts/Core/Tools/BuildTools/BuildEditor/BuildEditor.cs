#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Game.Editor
{
    /// <summary>
    /// 打包工具
    /// </summary>
    public static class BuildEditor
    {
        [UnityEditor.MenuItem("EditorTools/Build/BuildAndroid")]
        private static void BuildAndroid()
        {
            Build(false,false);
        }
        [UnityEditor.MenuItem("EditorTools/Build/BuildAndRunAndroid")]
        private static void BuildAndRunAndroid()
        {
            Build(false,true);
        }
        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="isDevelopment">是否是开发者模式</param>
        private static void Build(bool isDevelopment,bool run) 
        {
            BuildTarget mBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            if (mBuildTarget != BuildTarget.Android)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                Debug.Log("转换平台");
                return;
            }
            //设置参数
            PlayerSettings.companyName = BuildData.ComponentName;
            PlayerSettings.Android.useCustomKeystore = true;//不使用正式签名
            PlayerSettings.productName = BuildData.AppName;
            PlayerSettings.applicationIdentifier = "com." + BuildData.ComponentName + "."+ BuildData.ProductName;//包名
            PlayerSettings.bundleVersion = BuildData.Version;//版本号
            PlayerSettings.Android.bundleVersionCode = BuildData.VersionCode;
            //秘钥相关
            PlayerSettings.Android.keystoreName = BuildData.KeystorePath; // 路径
            PlayerSettings.Android.keystorePass = BuildData.KeystorePassword; // 密钥密码
            PlayerSettings.Android.keyaliasName = BuildData.keyaliasName; // 密钥别名
            PlayerSettings.Android.keyaliasPass = BuildData.keyaliasPassword;// 密钥密码

            string outputPath = Application.dataPath + "/../Build/";
            BuildOptions buildOption = BuildOptions.None;
            string apk;
            if (isDevelopment)
            {
                buildOption |= BuildOptions.Development;
                apk = outputPath + BuildData.ProductName + "_debug_";
            }
            else
            {
                buildOption &= ~BuildOptions.Development;
                apk = outputPath  + BuildData.ProductName + "_release_";
            }
            apk += DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss")+"_V"  + BuildData.Version + ".apk";
            try
            {
                BuildReport report = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, apk, BuildTarget.Android, buildOption);
                switch (report.summary.result)
                {
                    case BuildResult.Cancelled:
                        Debug.Log("取消打包->Android");
                        break;
                    case BuildResult.Failed:
                        string msg = "";
                        foreach (var file in report.files)
                        {
                            msg += file.path + "\n";
                        }
                        foreach (var step in report.steps)
                        {
                            foreach (var stepmsg in step.messages)
                            {
                                msg += "\n" + stepmsg.content;
                            }

                            msg += "\n";
                        }
                        Debug.LogError("打包失败->Android：【" + msg + "】");
                        break;
                    case BuildResult.Unknown:
                        Debug.LogError("打包失败->Android：未知错误：");
                        break;
                    case BuildResult.Succeeded:
                        Debug.Log("------------- 结束 BuildAPK -------------");
                        System.Diagnostics.Process.Start(outputPath);
                        if (run)
                        {
                            SpawnInstallApkBat(apk);
                            BatTools.RunMyBat(BuildData.InstallApkFileName, BuildData.InstallApkBatDir); 
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("打包失败->Android：" + e.Message);
            }
        }
        private static void SpawnInstallApkBat(string apkPath)
        {
            string str = @"cd C:\Unity\2020.3.32f1c1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools" +
                "\n"+
                @"adb install "+ apkPath;
            File.WriteAllText(Path.Combine(BuildData.InstallApkBatDir,BuildData.InstallApkFileName), str);
        }

    }
} 
#endif
