using UnityEngine;

[XLua.ReflectionUse]
public class UIBaseSingleton<T> : UIBase<T> where T : UIBaseSingleton<T>, new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    public override void DoAwake()
    {
        base.DoAwake();
        _instance = gameObject.GetComponent<T>();
    }

    public override void DoOnEnable()
    {
        base.DoOnEnable();
        if (_instance == null)
        {
            _instance = gameObject.GetComponent<T>();
        }
    }

    public override void DoOnDisable()
    {
        base.DoOnDisable();
        _instance = null;
    }

    public override void DoOnDestory()
    {
        base.DoOnDestory();
        _instance = null;
    }

    public static void Clear()
    {
        _instance = null;
    }
}

