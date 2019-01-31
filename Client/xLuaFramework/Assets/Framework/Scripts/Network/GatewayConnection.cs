using Debuger;
using GameFrame;

[XLua.ReflectionUse]
public class GatewayConnection : AuthConnectionClient
{
    public GatewayConnection()
    {
        protocolSecurity = false;
    }
    protected override void OnConnected()
    {
        base.OnConnected();
    }
    protected override void OnAuthSuccessed()
    {
        DebugerHelper.Log("連接服務器成功...", DebugerHelper.LevelType.Debug);
        ConnServer.m_IsConnectServer = true;
        ConnServer.m_IsConnecting = false;
        ConnServer.m_WaitServerMsgCount = 0;
        //InitGame.Instance.text.text = "連接服務器成功";
        LuaFunctionManager.OnConnectServerSucceed();
        //LuaFunctionManager.OnGetServerConfig(receiverJsonString);
    }
    protected override void OnDisconnected()
    {
        ConnServer.m_WaitServerMsgCount++;
        ConnServer.m_IsConnectServer = false;
        DebugerHelper.Log("断开服務器成功...", DebugerHelper.LevelType.Debug);
    }
    protected override void DefaultHandleMessage(NetworkMessage message)
    {
        if (ConnServer.m_WaitServerMsgCount > 0)
            ConnServer.m_WaitServerMsgCount--;
        LuaFunctionManager.OnReceiveServerMsg(message.cmd, message.readString());
    }
}


