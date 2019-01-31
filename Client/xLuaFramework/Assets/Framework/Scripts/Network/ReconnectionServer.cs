using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 断线重连
/// </summary>
[XLua.ReflectionUse]
public class ReconnectionServer : MonoBehaviour
{
    public static float intervalTime = 3.0f;//重新连接时间
    static NetworkReachability CurNetworkType = NetworkReachability.NotReachable;

    void Awake()
    {
        CurNetworkType = Application.internetReachability;
    }
    // Update is called once per frame
    void Update()
    {
        if (LocalData.isOtherLogin) return;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            CurNetworkType = NetworkReachability.NotReachable;
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                //GameData.ResultCodeStr = "本机没有网络数据";
                //UIManager.Instance.ShowUIPanel(UIPaths.ResultCodeDialog, OpenPanelType.MinToMax);
            }
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            if (CurNetworkType != NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                ConnServer.DisconnectServer();
                ConnServer.ConnectionServer();
                intervalTime = 0;
                CurNetworkType = NetworkReachability.ReachableViaCarrierDataNetwork;
            }
            else
            {
                if (ConnServer.m_IsConnecting) return;
                if (!ConnServer.m_IsConnectServer)//断开服务器
                {
                    if (intervalTime <= 0)
                    {
                        intervalTime = 3;
                        ConnServer.ConnectionServer();
                    }
                    else
                    {
                        intervalTime -= Time.deltaTime;
                    }
                }
            }
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            if (CurNetworkType != NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                ConnServer.DisconnectServer();
                ConnServer.ConnectionServer();
                intervalTime = 0;
                CurNetworkType = NetworkReachability.ReachableViaLocalAreaNetwork;

            }
            else
            {
                if (ConnServer.m_IsConnecting) return;
                if (!ConnServer.m_IsConnectServer)//断开服务器
                {
                    if (intervalTime <= 0)
                    {
                        intervalTime = 3;
                        ConnServer.m_WaitServerMsgCount = 0;
                        ConnServer.ConnectionServer();
                    }
                    else
                    {
                        intervalTime -= Time.deltaTime;
                    }
                }
            }
        }
    }
}
