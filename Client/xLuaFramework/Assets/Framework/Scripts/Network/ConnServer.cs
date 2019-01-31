using Debuger;
using GameFrame;
using UnityEngine;

[XLua.ReflectionUse]
public class ConnServer : MonoBehaviour
{
    public static class global
    {
        public static TcpClient Tcp_gateway;        //作为客户端 连接 GatewayServer 
    }
    public class GatewayClient : TcpClient
    {
        protected override void onConnectedFailed(string ip, ushort port)
        {
            connect<GatewayConnection>(ip, port);
        }
    }
    public static bool m_IsConnectServer = false;
    public static bool m_IsConnecting = false;
    public static int m_WaitServerMsgCount = 0;//消息计数
    public static bool m_IsIpv6 = false;

    private float waitMessageTime = 1;

    void Update()
    {
        if (LocalData.isOtherLogin) return;

        Connection.update();
        if (m_WaitServerMsgCount <= 0)
        {
            if (WaitServerTimeout.Instance != null)
            {
                WaitServerTimeout.Instance.DoDispose();
            }
            waitMessageTime = 1;
        }
        else if (m_WaitServerMsgCount > 0 && WaitServerTimeout.Instance == null)
        {
            if (waitMessageTime <= 0)
            {
                GameObject preObj = Resources.Load<GameObject>("WaitServerTimeout");
                GameObject obj = Instantiate(preObj);
                obj.transform.parent = GameObject.Find("2DUIRoot").transform;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;

                waitMessageTime = 1;
            }
            else
            {
                waitMessageTime -= Time.deltaTime;
            }
        }
    }
    #region 连接游戏服务器
    public static void ConnectionServer()
    {
        if (ServerInfo.Data.GateAddress == null) return;
        if (ServerInfo.Data.GatePort == 0) return;
        string ip = ToolsFunc.GetServerIP(ServerInfo.Data.GateAddress);
        ushort port = ServerInfo.Data.GatePort;
        DebugerHelper.Log("链接一次", DebugerHelper.LevelType.Debug);

        m_IsConnecting = true;
        global.Tcp_gateway = new GatewayClient();//
        if (m_IsIpv6)
            global.Tcp_gateway.connectIpv6<GatewayConnection>(ip, port);
        else
            global.Tcp_gateway.connect<GatewayConnection>(ip, port);
    }
    public static void DisconnectServer()
    {
        m_IsConnecting = false;
        if (global.Tcp_gateway != null)
        {
            DebugerHelper.Log("断开一次", DebugerHelper.LevelType.Debug);
            Connection conn = global.Tcp_gateway[0];
            if (conn != null)
                conn.close();
        }
    }
    #endregion
    void OnDestroy()
    {
        DisconnectServer();
    }
}
