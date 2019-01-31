using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class BuildToolEditor
{
    private static List<string> _listScenes = new List<string>();

    /// <summary>
    /// 打包之前需要做的事情
    /// </summary>
    [MenuItem("Tools/Build Tool/Before Build")]
    public static void BeforeBuild()
    {
        //1.Resources文件夹重命名 防止资源Build进安装包中
        string path = Application.dataPath + "/MyAssets/Resources";
        string targetPath = Application.dataPath + "/MyAssets/Before_Resources";
        if (Directory.Exists(path))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            dirInfo.MoveTo(targetPath);
        }
        //2.Scenes in Bulid中删除不需要的场景
        _listScenes.Clear();
        GetScenes(Application.dataPath + "/MyAssets/Scenes", ref _listScenes);
        ClearScenes();
        EditorBuildSettingsScene[] arrScenes = new EditorBuildSettingsScene[_listScenes.Count];
        for (int i = 0; i < arrScenes.Length; i++)
        {
            arrScenes[i] = new EditorBuildSettingsScene(_listScenes[i], true);
        }
        EditorBuildSettings.scenes = arrScenes;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("打包前准备完毕");
    }

    /// <summary>
    /// 打包完成需要做的事情
    /// </summary>
    [MenuItem("Tools/Build Tool/After Build")]
    public static void AfterBuild()
    {
        //1.之前改掉的Resources文件夹名改回来
        string path = Application.dataPath + "/MyAssets/Before_Resources";
        string targetPath = Application.dataPath + "/MyAssets/Resources";
        if (Directory.Exists(path))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            dirInfo.MoveTo(targetPath);
        }
        //2.Scenes in Bulid中更新所有场景
        _listScenes.Clear();
        GetScenes(Application.dataPath + "/MyAssets/Scenes", ref _listScenes);
        UpdateScenes();
        EditorBuildSettingsScene[] arrScenes = new EditorBuildSettingsScene[_listScenes.Count];
        for (int i = 0; i < arrScenes.Length; i++)
        {
            arrScenes[i] = new EditorBuildSettingsScene(_listScenes[i], true);
        }
        EditorBuildSettings.scenes = arrScenes;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("打包后恢复数据");
    }

    /// <summary>
    /// 打包完成需要做的事情
    /// </summary>
    [PostProcessBuild(1)]
    public static void AfterBuild(BuildTarget target, string pathToBuiltProject)
    {
        //AfterBuild();
        Debug.Log("打包完成: " + pathToBuiltProject);
    }

    //获取场景文件
    private static void GetScenes(string sPath, ref List<string> dirs)
    {
        foreach (string path in Directory.GetFiles(sPath))
        {
            if (Path.GetExtension(path) == ".unity")
            {
                string pathNormal = path.Replace("\\", "/");
                int nIndex = pathNormal.IndexOf("Assets/", StringComparison.Ordinal);
                string sName = pathNormal.Substring(nIndex);
                dirs.Add(sName);
            }
        }
        if (Directory.GetDirectories(sPath).Length > 0)
        {
            foreach (string path in Directory.GetDirectories(sPath))
                GetScenes(path, ref dirs);
        }
    }

    //清理场景
    private static void ClearScenes()
    {
        if (_listScenes == null)
            return;

        string sUpdate = string.Empty;
        for (int i = _listScenes.Count - 1; i >= 0; i--)
        {
            if (_listScenes[i].Contains("01_InitGame"))
            {
                sUpdate = _listScenes[i];
                break;
            }
        }
        _listScenes.Clear();
        if (sUpdate != null)
            _listScenes.Add(sUpdate);
    }

    //更新场景
    private static void UpdateScenes()
    {
        if (_listScenes == null)
            return;
        
        _listScenes.Sort((a, b) =>
        {
            string[] arrA = a.Split('/');
            string[] arrB = b.Split('/');
            string sA = arrA[arrA.Length - 1].Substring(0, 2);
            string sB = arrB[arrB.Length - 1].Substring(0, 2);
            int nA = int.Parse(sA);
            int nB = int.Parse(sB);
            return nA - nB;
        });
    }
}