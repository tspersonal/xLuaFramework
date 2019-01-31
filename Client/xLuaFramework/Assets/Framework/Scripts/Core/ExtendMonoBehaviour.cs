using UnityEngine;
using XLua;

/// <summary>
/// Mono的基类
/// </summary>
[XLua.ReflectionUse]
public class ExtendMonoBehaviour : MonoBehaviour
{
    public LuaFunction LuaDoAwake;
    public LuaFunction LuaDoOnEnable;
    public LuaFunction LuaDoStart;
    public LuaFunction LuaDoFixedUpdate;
    public LuaFunction LuaDoUpdate;
    public LuaFunction LuaDoOnDisable;
    public LuaFunction LuaDoOnDestory;
    public LuaFunction LuaDoOnApplicationFocus;
    public LuaFunction LuaDoOnApplicationPause;
    public LuaFunction LuaDoOnApplicationQuit;

    public LuaFunction LuaDoAddListener;
    public LuaFunction LuaDoRemoveListener;
    public LuaFunction LuaDoSetData;
    public LuaFunction LuaDoResetData;
    public LuaFunction LuaDoClearData;

    void Awake()
    {
        DoAwake();
    }

    void OnEnable()
    {
        DoAddListener();
        DoOnEnable();
    }

    void Start()
    {
        DoStart();
    }

    void FixedUpdate()
    {
        DoFixedUpdate();
    }

    void Update()
    {
        DoUpdate();
    }

    void OnDisable()
    {
        DoRemoveListener();
        DoOnDisable();
    }

    void OnDestroy()
    {
        DoClearData();
        DoOnDestory();
    }

    void OnApplicationFocus(bool focusStatus)
    {
        DoOnApplicationFocus(focusStatus);
    }

    private void OnApplicationPause(bool pause)
    {
        DoOnApplicationPause(pause);
    }

    void OnApplicationQuit()
    {
        DoOnApplicationQuit();
    }

    public virtual void DoAwake()
    {
        if (null != LuaDoAwake)
        {
            LuaDoAwake.Call();
        }
    }

    public virtual void DoStart()
    {
        if (null != LuaDoStart)
        {
            LuaDoStart.Call();
        }
    }

    public virtual void DoFixedUpdate()
    {
        if (null != LuaDoFixedUpdate)
        {
            LuaDoFixedUpdate.Call();
        }
    }

    public virtual void DoUpdate()
    {
        if (null != LuaDoUpdate)
        {
            LuaDoUpdate.Call();
        }
    }

    public virtual void DoOnEnable()
    {
        if (null != LuaDoOnEnable)
        {
            LuaDoOnEnable.Call();
        }
    }

    public virtual void DoOnDisable()
    {
        if (null != LuaDoOnDisable)
        {
            LuaDoOnDisable.Call();
        }
    }

    public virtual void DoOnDestory()
    {
        if (null != LuaDoOnDestory)
        {
            LuaDoOnDestory.Call();
        }
    }

    public virtual void DoOnApplicationFocus(bool focusStatus)
    {
        if (null != LuaDoOnApplicationFocus)
        {
            LuaDoOnApplicationFocus.Call(focusStatus);
        }
    }

    public virtual void DoOnApplicationPause(bool pause)
    {
        if (null != LuaDoOnApplicationPause)
        {
            LuaDoOnApplicationPause.Call(pause);
        }
    }

    public virtual void DoOnApplicationQuit()
    {
        if (null != LuaDoOnApplicationQuit)
        {
            LuaDoOnApplicationQuit.Call();
        }
    }
    
    /// <summary>
    /// 注册监听事件，用于广播事件
    /// </summary>
    protected virtual void DoAddListener()
    {
        if (null != LuaDoAddListener)
        {
            LuaDoAddListener.Call();
        }
    }

    /// <summary>
    /// 取消注册监听事件，用于广播事件
    /// </summary>
    protected virtual void DoRemoveListener()
    {
        if (null != LuaDoRemoveListener)
        {
            LuaDoRemoveListener.Call();
        }
    }

    /// <summary>
    /// 带入数据
    /// </summary>
    /// <param name="obj"></param>
    public virtual void DoSetData(object obj = null)
    {
        if (null != LuaDoSetData)
        {
            LuaDoSetData.Call();
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public virtual void DoResetData()
    {
        if (null != LuaDoResetData)
        {
            LuaDoResetData.Call();
        }
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    public virtual void DoClearData()
    {
        if (null != LuaDoClearData)
        {
            LuaDoClearData.Call();
        }
    }

    /// <summary>
    /// 显示
    /// </summary>
    public virtual void DoShow()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public virtual void DoHide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void DoDispose()
    {
        DestroyImmediate(gameObject);
    }
}
