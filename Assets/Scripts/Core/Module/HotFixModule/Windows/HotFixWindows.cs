#if UNITY_EDITOR
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    class HotFixWindows : EditorWindow
    {
        [MenuItem("EditorTools/BuildHotfix")]
        static void Init()
        {
            HotFixWindows hotFixEditor = (HotFixWindows)EditorWindow.GetWindow(typeof(HotFixWindows), false, "选择MD5", true);
            hotFixEditor.minSize = Vector2.one * 500;
            hotFixEditor.position = new Rect(Vector2.zero, Vector2.one * 300);
            hotFixEditor.Show();
        }
        OpenFileName ofn = null;
        string md5Path = "";
        string version = "1";
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            md5Path = EditorGUILayout.TextField("选择MD5:", md5Path, GUILayout.Width(300), GUILayout.Height(20));
            if (GUILayout.Button("选择MD5文件", GUILayout.Width(150), GUILayout.Height(20)))
            {
                ofn = new OpenFileName();
                ofn.structSize = Marshal.SizeOf(ofn);
                ofn.filter = "选择MD5(\0*.bytes\0)";
                ofn.file = new string(new char[256]);
                ofn.maxFile = ofn.file.Length;
                ofn.fileTitle = new string(new char[64]);
                ofn.maxFileTitle = ofn.fileTitle.Length;
                ofn.initialDir = Application.dataPath + "/../Version";//默认路径
                ofn.title = "选择MD5";
                //注意 一下项目不一定要全选 但是0x00000008项不要缺少
                ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
                if (OpenWindowsTools.GetSaveFileName(ofn))
                {
                    md5Path = ofn.file;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            version = EditorGUILayout.TextField("选择版本", version, GUILayout.Width(300), GUILayout.Height(20));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("开始打包热更新", GUILayout.Width(150), GUILayout.Height(20)))
            {
                if (File.Exists(md5Path) && md5Path.EndsWith(".bytes"))
                {
                    AssetBundleEditor.PackageAssetBundle(true, md5Path, version);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif