using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[XLua.ReflectionUse]
public class ServerInfo
{
    public string GameId;//标识
    public string DefineName;//定义
    public bool IsMainServer;//是否主服
    public string GateAddress;//服务器地址
    public ushort GatePort;//服务器端口
    public string ServerVersionCode;//服务器版本号
    public string ServerUpdateMessage;//服务器上次更新信息
    public string UpdateAndroidUrl;//android更新地址
    public string UpdateIosUrl;//ios更新地址
    public bool OnAppleCheck;//是否在苹果审核期间
    public bool AuthCodeCheckOpened;//是否开启验证码
    public bool ClientLogOpened;//是否开启客户端log
    public string UpLoadPictureUrl;//客户端图片上传地址
    public string UpLoadSoundUrl;//客户端语音上传地址
    public string UpLoadResourceUrl;//客户端资源上传地址
    public string DownLoadPictureUrl;//客户端语音下载地址
    public string DownLoadSoundUrl;//客户端语音下载地址
    public string DownLoadResourceUrl;//客户端资源下载地址

    public uint ConnectionCount;//连接数

    public static ServerInfo Data = new ServerInfo();
}
