using System.IO;
using UnityEngine;
using XLua;

/// <summary>
/// 路径
/// </summary>
[XLua.ReflectionUse]
public class PathUtil
{
    /// <summary>
    /// 获取assetbundle的输出目录
    /// </summary>
    /// <returns></returns>
    public static string GetAssetBundleOutPath()
    {
        string outPath = GetPlatformPath() + "/" + GetPlatformName();

        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);

        return outPath;
    }

    /// <summary>
    /// 获取files文件输出路劲
    /// </summary>
    /// <returns></returns>
    public static string GetFilesOutPath()
    {
        string outPath = GetPlatformPath() + "/";

        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);

        return outPath;
    }

    /// <summary>
    /// 自动获取对应平台的路径
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
                return Application.streamingAssetsPath;
            case RuntimePlatform.WindowsEditor:
                return Application.streamingAssetsPath;
            case RuntimePlatform.Android:
                return Application.persistentDataPath;
            case RuntimePlatform.IPhonePlayer:
                if (ServerInfo.Data.OnAppleCheck)
                    return Application.streamingAssetsPath;
                else
                    return Application.persistentDataPath;
            default:
                return Application.streamingAssetsPath;
        }
    }

    /// <summary>
    /// 获取对应平台的名字
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformName()
    {
        //return "Android";
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "Windows";
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "IPhone";
            default:
                return null;
        }
    }
}
