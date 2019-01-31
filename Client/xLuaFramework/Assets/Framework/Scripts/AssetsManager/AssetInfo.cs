using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Object = UnityEngine.Object;

[XLua.ReflectionUse]
public class AssetInfo
{
    private string _sName;
    private string _sPath;
    private int _nAssetType;
    private Object _objAsset;

    private ResourceRequest _request;//用于异步加载
    public List<LuaFunction> ListListener;//用于回调事件

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sName">资源名字</param>
    /// <param name="sPath">资源路径</param>
    /// <param name="nAssetType">资源类型</param>
    /// <param name="objAsset"></param>
    public AssetInfo(string sName, string sPath, int nAssetType, Object objAsset)
    {
        this._sName = sName;
        this._sPath = sPath;
        this._nAssetType = nAssetType;
        this._objAsset = objAsset;
    }

    public string SName
    {
        get
        {
            return _sName;
        }

        set
        {
            _sName = value;
        }
    }

    public string SPath
    {
        get
        {
            return _sPath;
        }

        set
        {
            _sPath = value;
        }
    }

    public int NAssetType
    {
        get
        {
            return _nAssetType;
        }

        set
        {
            _nAssetType = value;
        }
    }

    public Object ObjAsset
    {
        get
        {
            return _objAsset;
        }

        set
        {
            _objAsset = value;
        }
    }

    public ResourceRequest Request
    {
        get
        {
            return _request;
        }

        set
        {
            _request = value;
        }
    }

    /// <summary>
    /// 同步加载返回Object
    /// </summary>
    public T LoadSync<T>(string sPath) where T : Object
    {
        return Resources.Load<T>(sPath);
    }

    /// <summary>
    /// 同步加载返回Object
    /// </summary>
    public T[] LoadAllSync<T>(string sPath) where T : Object
    {
        return Resources.LoadAll<T>(sPath);
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <returns></returns>
    public ResourceRequest LoadAsync(string sPath)
    {
        var req = Resources.LoadAsync(sPath);
        _request = req;
        return req;
    }

    /// <summary>
    /// 异步加载返回Object
    /// </summary>
    /// <returns></returns>
    public T LoadAsync<T>(string sPath) where T : Object
    {
        _request = Resources.LoadAsync(sPath);
        return (T)Request.asset;
    }

    /// <summary>
    /// 添加回调
    /// </summary>
    /// <param name="fun"></param>
    public void AddListener(LuaFunction fun)
    {
        if (ListListener == null)
            ListListener = new List<LuaFunction>();

        if (ListListener.Contains(fun))
            return;

        ListListener.Add(fun);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void UnLoadAsset()
    {
        if (_objAsset)
            Resources.UnloadAsset(_objAsset);
        _sName = "";
        _sPath = "";
        _nAssetType = -1;
        _objAsset = null;
        _request = null;
        ListListener = null;
    }
}
