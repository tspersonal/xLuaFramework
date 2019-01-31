public class LuaFunctionManager
{
    //接收服务器返回数据
    public static OnReceiveServerMessage OnReceiveServerMsg;
    //服务器连接成功
    public static OnConnectServerSucceed OnConnectServerSucceed;
    //获取服务器配置
    public static OnGetServerConfig OnGetServerConfig;
    //授权成功
    public static OnAuthHandler OnAuthHandler;
    //获取授权信息
    public static OnShowUserHandler OnShowUserHandler;
    //分享回调
    public static OnShareHandler OnShareHandler;
    //启动加载资源完毕
    public static OnLoadAbComplete OnLoadAbComplete;
    //加载场景完成
    public static OnLoadSceneComplete OnLoadSceneComplete;
}
