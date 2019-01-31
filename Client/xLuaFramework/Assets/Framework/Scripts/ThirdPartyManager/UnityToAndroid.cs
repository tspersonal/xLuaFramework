using Debuger;
using UnityEngine;

public class UnityToAndroid
{
    //打开相册	
    public static void OpenPhoto(string resultGameObjectName, string imageName)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("OpenPhoto", resultGameObjectName, imageName);
    }
    //获取子网掩码
    public static void GetMask(string resultGameObjectName)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("GetMask", resultGameObjectName);
    }
    //复制到剪切板
    public static void SetClipboard(string text)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("CopyTextToClipboard", jo, text);
    }
    //获取剪切板内容
    public static string GetClipboard()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string text = jo.Call<string>("GetTextFromClipboard", jo);
        if (text.Equals("null"))
        {
            DebugerHelper.Log("GetTextFromClipboard Android Return null", DebugerHelper.LevelType.Warning);
            return "";
        }
        else
            return text;
    }
}
