using UnityEngine;
using XLua;
//using cn.sharesdk.unity3d;
//using cn.sharesdk.unity3d;

[XLua.ReflectionUse]
public class ShareSdkHelper : SingletonMonoBehaviour<ShareSdkHelper>
{
    public override void DoStart()
    {
        base.DoStart();
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //InitSDK();
        }
    }

    //    #region 微信授权登录
    //    [HideInInspector]
    //    public ShareSDK ssdk;

    //    void InitSDK()
    //    {
    //        ssdk = gameObject.GetComponent<ShareSDK>();
    //        ssdk.authHandler = OnAuthResultHandler;
    //        ssdk.showUserHandler = OnGetUserInfoResultHandler;
    //        ssdk.shareHandler = OnShareResultHandler;
    //    }

    //    /// <summary>
    //    /// 分享截屏
    //    /// </summary>
    //    public void ShareCapture()
    //    {
    //        ShareContent content = new ShareContent();
    //        ScreenCapture.CaptureScreenshot("Shot4Share.png");
    //        //设置图片路径
    //        content.SetImagePath(Application.persistentDataPath + "/Shot4Share.png");
    //        content.SetShareType(ContentType.Image);

    //        //ssdk.ShowShareContentEditor(PlatformType.WeChat, content);
    //        ssdk.ShowPlatformList(new PlatformType[] { PlatformType.WeChat, PlatformType.WeChatMoments }, content, 100, 100);
    //    }
    //    /// <summary>
    //    /// 分享房间号
    //    /// </summary>
    //    /// <param name="roomID"></param>
    //    ///  <param name="titlePrefix">标题前缀</param>
    //    public void ShareRoomID(uint roomID, string text, string titlePrefix,string shareUrl)
    //    {
    //        ShareContent content = new ShareContent();
    //        content.SetTitle(titlePrefix + "房间号:" + roomID.ToString());
    //        content.SetText(text);
    //        ToolsFunc.SetShareIcon();
    //        content.SetImagePath(Application.persistentDataPath + "/icon.png");
    //        content.SetUrl(shareUrl);
    //        content.SetShareType(ContentType.Webpage);
    //        ssdk.ShowPlatformList(new PlatformType[] { PlatformType.WeChat, PlatformType.WeChatMoments }, content, 100, 100);
    //    }
    //    public void ShareURL(string text, string titlePrefix,string url)
    //    {
    //        ShareContent content = new ShareContent();
    //        content.SetTitle(titlePrefix);
    //        content.SetText(text);
    //        ToolsFunc.SetShareIcon();
    //        content.SetImagePath(Application.persistentDataPath + "/icon.png");
    //        content.SetUrl(url);
    //        content.SetShareType(ContentType.Webpage);
    //        ssdk.ShowPlatformList(new PlatformType[] { PlatformType.WeChat, PlatformType.WeChatMoments }, content, 100, 100);
    //    }

    //    public void ShareImageURL(string text, string imageURL)
    //    {
    //        ShareContent content = new ShareContent();
    //        content.SetTitle(text);
    //        content.SetImageUrl(imageURL);
    //        content.SetShareType(ContentType.Image);
    //        ssdk.ShowPlatformList(new PlatformType[] { PlatformType.WeChat, PlatformType.WeChatMoments }, content, 100, 100);
    //    }
    //    /// <summary>
    //    /// 登录授权
    //    /// </summary>
    //    public void Authorize()
    //    {
    //        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
    //        {
    //            ssdk.Authorize(PlatformType.WeChat);
    //        }
    //    }

    //    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    //    { 

    //        string strs = "";
    //        if (state == ResponseState.Success)
    //        {
    //            strs = MiniJSON.jsonEncode(data);
    //            //JsonData jd = JsonMapper.ToObject(strs);
    //            //.Instance.openID = jd["openid"].ToString();
    //            //.Instance.otherName = jd["nickname"].ToString();
    //            //Player.Instance.headID = jd["headimgurl"].ToString();
    //            //Player.Instance.sex = byte.Parse(jd["sex"].ToString());
    //            //UIManager.Instance.HideUIPanel(UIPaths.LoadingObj);
    //            //ClientToServerMsg.Send(Opcodes.Client_Character_Create, Player.Instance.openID, Player.Instance.otherName, Player.Instance.headID, (byte)Player.Instance.sex);
    //        }

    //        LuaFunctionManager.OnShowUserHandler((int)state,strs);
    //    }

    //    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable data)
    //    {
    //        LuaFunctionManager.OnAuthHandler((int)state);
    //        if (state == ResponseState.Success)
    //        {
    //            ssdk.GetUserInfo(PlatformType.WeChat);
    //        }
    //        else if (state == ResponseState.Fail)
    //        {
    //            //GameData.ResultCodeStr = "fail! throwable stack = " + data["stack"] + "; error msg = " + data["msg"];
    //            //UIManager.Instance.ShowUIPanel(UIPaths.ResultCodeDialog, OpenPanelType.MinToMax);
    //        }
    //    }

    //    /// <summary>
    //    /// 分享回调
    //    /// </summary>
    //    /// <param name="reqID"></param>
    //    /// <param name="state"></param>
    //    /// <param name="type"></param>
    //    /// <param name="data"></param>
    //    void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    //    {
    //        LuaFunctionManager.OnShareHandler((int)state);
    //        if (state == ResponseState.Success)
    //        {
    //            print("share successfully - share result :");
    //            print(MiniJSON.jsonEncode(result));
    //           // ClientToServerMsg.Send(Opcodes.Client_ShareSuccess);
    //        }
    //        else if (state == ResponseState.Fail)
    //        {
    //#if UNITY_ANDROID
    //            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
    //            //GameData.ResultCodeStr = "fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"];
    //#elif UNITY_IPHONE
    //            //GameData.ResultCodeStr = "fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"];
    //			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
    //#endif
    //        //UIManager.Instance.ShowUIPanel(UIPaths.ResultCodeDialog, OpenPanelType.MinToMax);
    //        }
    //        else if (state == ResponseState.Cancel)
    //        {
    //            print("cancel !");
    //        }
    //    }
    //    #endregion
}
