using UnityEngine;
using XLua;

/// <summary>
////// Lua脚本进行调用 用于对Lua脚本进行Mono的绑定
/// </summary>
[XLua.ReflectionUse]
public static class LuaMonoEvent
{
    /// <summary>
    /// 绑定mono事件
    /// </summary>
    /// <param name="sMonoEventType">事件类型</param>
    /// <param name="go">对象</param>
    /// <param name="luaFun">lua方法</param>
    public static void BindMonoEvent(string sMonoEventType, GameObject go, LuaFunction luaFun)
    {
        LuaMonoBehaviour luaMono = go.GetComponent<LuaMonoBehaviour>();
        if (luaMono == null)
        {
            luaMono = go.AddComponent<LuaMonoBehaviour>();
        }
        switch (sMonoEventType)
        {
            case "Awake":
                luaMono.LuaDoAwake = luaFun;
                break;
            case "OnEnable":
                luaMono.LuaDoOnEnable = luaFun;
                break;
            case "Start":
                luaMono.LuaDoStart = luaFun;
                break;
            case "FixedUpdate":
                luaMono.LuaDoFixedUpdate = luaFun;
                break;
            case "Update":
                luaMono.LuaDoUpdate = luaFun;
                break;
            case "OnDisable":
                luaMono.LuaDoOnDisable = luaFun;
                break;
            case "OnDestroy":
                luaMono.LuaDoOnDestory = luaFun;
                break;
            case "OnApplicationFocus":
                luaMono.LuaDoOnApplicationFocus = luaFun;
                break;
            case "OnApplicationPause":
                luaMono.LuaDoOnApplicationPause = luaFun;
                break;
            case "SetData":
                luaMono.LuaDoSetData = luaFun;
                break;
            case "ResetData":
                luaMono.LuaDoResetData = luaFun;
                break;
            case "ClearData":
                luaMono.LuaDoClearData = luaFun;
                break;
            default:
                luaFun.Dispose();
                break;
        }
    }

    /// <summary>
    /// 绑定UIBase事件
    /// </summary>
    /// <param name="sMonoEventType">事件类型</param>
    /// <param name="go">对象</param>
    /// <param name="luaFun">lua方法</param>
    public static void BindUiBaseEvent(string sMonoEventType, GameObject go, LuaFunction luaFun)
    {
        LuaUIBase luaUiBase = go.GetComponent<LuaUIBase>();
        if (luaUiBase == null)
        {
            luaUiBase = go.AddComponent<LuaUIBase>();
        }
        switch (sMonoEventType)
        {
            case "Awake":
                luaUiBase.LuaDoAwake = luaFun;
                break;
            case "OnEnable":
                luaUiBase.LuaDoOnEnable = luaFun;
                break;
            case "Start":
                luaUiBase.LuaDoStart = luaFun;
                break;
            case "FixedUpdate":
                luaUiBase.LuaDoFixedUpdate = luaFun;
                break;
            case "Update":
                luaUiBase.LuaDoUpdate = luaFun;
                break;
            case "OnDisable":
                luaUiBase.LuaDoOnDisable = luaFun;
                break;
            case "OnDestroy":
                luaUiBase.LuaDoOnDestory = luaFun;
                break;
            case "OnApplicationFocus":
                luaUiBase.LuaDoOnApplicationFocus = luaFun;
                break;
            case "OnApplicationPause":
                luaUiBase.LuaDoOnApplicationPause = luaFun;
                break;
            case "SetData":
                luaUiBase.LuaDoSetData = luaFun;
                break;
            case "ResetData":
                luaUiBase.LuaDoResetData = luaFun;
                break;
            case "ClearData":
                luaUiBase.LuaDoClearData = luaFun;
                break;
            case "Register":
                luaUiBase.LuaDoRegister = luaFun;
                break;
            default:
                luaFun.Dispose();
                break;
        }
    }
}
