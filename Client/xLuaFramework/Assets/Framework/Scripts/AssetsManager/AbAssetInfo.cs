using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

/// <summary>
/// AB资源信息
/// </summary>
[XLua.ReflectionUse]
public class AbAssetInfo
{
    private string _sAbName;
    private string _sAbPath;
    private int _nAssetType;
    private AssetBundle _abAsset;

    private bool _bInstantiate = false;//是否需要实例化
    private AssetBundleCreateRequest _request;//用于异步加载
    public List<LuaFunction> ListListener;//用于回调事件

    public string SAbName
    {
        get
        {
            return _sAbName;
        }

        set
        {
            _sAbName = value;
        }
    }

    public string SAbPath
    {
        get
        {
            return _sAbPath;
        }

        set
        {
            _sAbPath = value;
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

    public AssetBundle AbAsset
    {
        get
        {
            return _abAsset;
        }

        set
        {
            _abAsset = value;
        }
    }

    public bool BInstantiate
    {
        get
        {
            return _bInstantiate;
        }

        set
        {
            _bInstantiate = value;
        }
    }

    public AssetBundleCreateRequest Request
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

    public AbAssetInfo(string sSAbName, string sSAbPath, int nAssetType, AssetBundle abAsset)
    {
        this._sAbName = sSAbName;
        this._sAbPath = sSAbPath;
        this._nAssetType = nAssetType;
        this._abAsset = abAsset;
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <returns></returns>
    public AssetBundleCreateRequest LoadAsync(string sAbName, string sAbPath)
    {
        var req = AssetBundle.LoadFromFileAsync(sAbPath);
        _request = req;
        return req;
    }

    /// <summary>
    /// 添加回调
    /// </summary>
    /// <param name="fun"></param>
    /// <param name="bInstantiate"></param>
    public void AddListener(LuaFunction fun, bool bInstantiate = false)
    {
        _bInstantiate = bInstantiate;

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
        if (_abAsset)
            _abAsset.Unload(true);
        _sAbName = "";
        _sAbPath = "";
        _nAssetType = -1;
        _abAsset = null;
        _request = null;
        ListListener = null;
    }
}
