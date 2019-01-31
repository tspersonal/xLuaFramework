using Debuger;
using UnityEngine;
using XLua;

[CSharpCallLua]
public delegate void OnReceiveServerMessage(int cmd, string jsonString);
[CSharpCallLua]
public delegate void OnConnectServerSucceed();
[CSharpCallLua]
public delegate void OnGetServerConfig(string jsonString);
[CSharpCallLua]
public delegate void OnAuthHandler(int state);
[CSharpCallLua]
public delegate void OnShowUserHandler(int state, string jsonString);
[CSharpCallLua]
public delegate void OnShareHandler(int state);
[CSharpCallLua]
public delegate void OnLoadAbComplete();
[CSharpCallLua]
public delegate void OnLoadSceneComplete(string sceneName);
[CSharpCallLua]
public delegate void OnVoiceResult(string fileName, float clipLength);

public class CSharpCallLuaFunc
{
    public CSharpCallLuaFunc()
    {
        DebugerHelper.Log("Blind Delegate What C# Call XLua", DebugerHelper.LevelType.Critical);
        LuaFunctionManager.OnReceiveServerMsg = LuaManager.luaEnv.Global.GetInPath<OnReceiveServerMessage>("OnReceiveServerMsg");
        LuaFunctionManager.OnConnectServerSucceed = LuaManager.luaEnv.Global.GetInPath<OnConnectServerSucceed>("OnConnectServerSucceed");
        LuaFunctionManager.OnGetServerConfig = LuaManager.luaEnv.Global.GetInPath<OnGetServerConfig>("OnGetServerConfig");
        LuaFunctionManager.OnAuthHandler = LuaManager.luaEnv.Global.GetInPath<OnAuthHandler>("OnAuthHandler");
        LuaFunctionManager.OnShowUserHandler = LuaManager.luaEnv.Global.GetInPath<OnShowUserHandler>("OnShowUserHandler");
        LuaFunctionManager.OnShareHandler = LuaManager.luaEnv.Global.GetInPath<OnShareHandler>("onShareResultHandler");
        LuaFunctionManager.OnLoadAbComplete = LuaManager.luaEnv.Global.GetInPath<OnLoadAbComplete>("OnLoadAbComplete");
        LuaFunctionManager.OnLoadSceneComplete = LuaManager.luaEnv.Global.GetInPath<OnLoadSceneComplete>("OnLoadSceneComplete");
    }
}


