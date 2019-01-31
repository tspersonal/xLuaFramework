using UnityEngine;
using XLua;

[XLua.ReflectionUse]
public class YzHelper
{
    public static LuaFunction photoLuaFunction;
    //打开相册
    public static void OpenPhoto(string resultGameObjectName, string imageName, LuaFunction luaFunction)
    {
        photoLuaFunction = luaFunction;
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityToAndroid.OpenPhoto(resultGameObjectName, imageName);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            UnityToIOS.OpenPhoto(resultGameObjectName, imageName);
        }
    }
    //获取子网掩码
    public static void GetMask(string resultGameObjectName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityToAndroid.GetMask(resultGameObjectName);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
    }
    //复制到粘贴板
    public static void SetClipboard(string text)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityToAndroid.SetClipboard(text);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            UnityToIOS.SetClipboard(text);
        }
        else
        {
            UnityToWindows.SetClipboard(text);
        }
    }
    //获取剪切板内容
    public static string GetClipboard()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return UnityToAndroid.GetClipboard();
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return UnityToIOS.GetClipboard();
        }
        else
        {
            return UnityToWindows.GetClipboard();
        }
    }
}
