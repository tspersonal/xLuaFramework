using System.Collections.Generic;
using UnityEngine;
using XLua;

/// <summary>
/// 提供泛型的UI基类
/// </summary>
/// <typeparam name="T"></typeparam>
[XLua.ReflectionUse]
public class UIBase<T> : ExtendMonoBehaviour where T : UIBase<T>, new()
{
    public LuaFunction LuaDoRegister;
   
    public override void DoAwake()
    {
        base.DoAwake();
        DoRegister();
    }

    /// <summary>
    /// 注册监听事件，例如Button、Input等
    /// </summary>
    protected virtual void DoRegister()
    {
        if (null != LuaDoRegister)
        {
            LuaDoRegister.Call();
        }
    }
}
