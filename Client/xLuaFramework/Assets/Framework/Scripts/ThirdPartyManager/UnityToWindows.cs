using UnityEngine;

public class UnityToWindows
{
    //打开相册	
    public static void OpenPhoto(string resultGameObjectName, string imageName)
    {
    }

    //获取子网掩码
    public static void GetMask(string resultGameObjectName)
    {
    }

    //复制到剪切板
    public static void SetClipboard(string text)
    {
        GUIUtility.systemCopyBuffer = text;
    }

    //获取剪切板内容
    public static string GetClipboard()
    {
        return GUIUtility.systemCopyBuffer;
    }
}
