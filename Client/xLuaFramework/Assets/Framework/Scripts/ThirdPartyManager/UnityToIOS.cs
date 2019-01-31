using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class UnityToIOS
{
    [DllImport("__Internal")]
    private static extern void IOS_OpenAlbum(string url, string _imageName);

    [DllImport("__Internal")]
    private static extern void IOS_CopyText(string text);

    [DllImport("__Internal")]
    private static extern string IOS_GetPasteboard();

    //打开相册
    public static void OpenPhoto(string gameObjectName, string imageName)
    {
        IOS_OpenAlbum(gameObjectName, imageName);
    }
    //复制到剪切板
    public static void SetClipboard(string text)
    {
        IOS_CopyText(text);
    }
    //获取剪切板内容
    public static string GetClipboard()
    {
        return IOS_GetPasteboard();
    }
}
