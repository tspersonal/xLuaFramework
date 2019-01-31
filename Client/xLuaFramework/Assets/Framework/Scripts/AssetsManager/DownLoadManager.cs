using Debuger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public enum DownType
{
    None,
    Text,
    Texture2D,
}

/// <summary>
/// 下载资源的单例类，挂在Init
/// </summary>
[XLua.ReflectionUse]
public class DownLoadManager : SingletonMonoBehaviour<DownLoadManager>
{
    /// <summary>
    /// 缓存文本
    /// </summary>
    private Dictionary<string, string> _dicCacheText = new Dictionary<string, string>();
    /// <summary>
    /// 缓存2D纹理
    /// </summary>
    private Dictionary<string, Texture2D> _dicCacheTexture2D = new Dictionary<string, Texture2D>();
    //相同的资源进行缓存，当一个加载完毕，所有的即被赋值
    private Dictionary<string, List<LuaFunction>> _dicCacheAction = new Dictionary<string, List<LuaFunction>>();


    #region 下载文本

    /// <summary>
    /// 下载文本
    /// </summary>
    /// <param name="sPath">路径</param>
    /// <param name="luaFun">lua回调</param>
    /// <param name="bCover">是否覆盖之前相同路径的下载</param>
    public void DownLoadText(string sPath, LuaFunction luaFun, bool bCover = false)
    {
        if (_dicCacheText.ContainsKey(sPath))
        {
            if (!bCover)
            {
                if (luaFun != null)
                {
                    luaFun.Call(_dicCacheText[sPath]);
                }
                return;
            }
        }
        StartCoroutine(IeDownLoadText(sPath, luaFun));
    }

    private IEnumerator IeDownLoadText(string sPath, LuaFunction luaFun)
    {
        WWW www = new WWW(sPath);
        yield return www;
        if (www.isDone && www.error == null)
        {
            string value = www.text;
            if (_dicCacheText.ContainsKey(sPath))
            {
                _dicCacheText[sPath] = value;
            }
            else
            {
                _dicCacheText.Add(sPath, value);
            }
            if (luaFun != null)
            {
                luaFun.Call(value);
            }
        }
        else if (!www.isDone)
        {
            DebugerHelper.Log("下载Text未成功！", DebugerHelper.LevelType.Error);
        }
        else if (www.error != null)
        {
            DebugerHelper.Log("下载Text错误：" + www.error, DebugerHelper.LevelType.Error);
        }
    }

    #endregion

    #region 下载2D纹理

    public void DownLoadTexture2D(string sPath, LuaFunction luaFun, bool bCover = false)
    {
        if (_dicCacheAction.ContainsKey(sPath))
        {
            //如果有资源正在被加载
            if (_dicCacheTexture2D.ContainsKey(sPath))
            {
                //如果该资源正在被加载，但是已经被加载完毕，则直接回调
                Texture2D tex = _dicCacheTexture2D[sPath];
                if (tex != null)
                {
                    //资源不为空的话
                    if (!bCover)
                    {
                        //如果不被覆盖
                        if (luaFun != null)
                        {
                            luaFun.Call(tex);
                        }
                        return;
                    }
                }
                //重新加载
                if (_dicCacheAction[sPath] == null)
                    _dicCacheAction[sPath] = new List<LuaFunction>();
                if (!_dicCacheAction[sPath].Contains(luaFun))
                    _dicCacheAction[sPath].Add(luaFun);

            }
            else
            {
                //如果该资源正在被加载，但是却没被加载完毕，则保存改回调，等待加载完毕
                if (_dicCacheAction[sPath] == null)
                    _dicCacheAction[sPath] = new List<LuaFunction>();
                if (!_dicCacheAction[sPath].Contains(luaFun))
                    _dicCacheAction[sPath].Add(luaFun);
                //如果这个是第一个入池的话 那么重新加载
                if (_dicCacheAction[sPath].Count > 1)
                    return;
            }
        }
        else
        {
            //如果没有资源正在被加载
            List<LuaFunction> list = new List<LuaFunction>();
            list.Add(luaFun);
            _dicCacheAction.Add(sPath, list);
        }

        StartCoroutine(IeDownLoadTexture2D(sPath));
    }

    private IEnumerator IeDownLoadTexture2D(string sPath)
    {
        WWW www = new WWW(sPath);
        yield return www;
        if (www.isDone && www.error == null)
        {
            Texture2D value = www.texture;
            if (value != null)
            {
                //对所有的加载入口进行回调
                for (var i = 0; i < _dicCacheAction[sPath].Count; i++)
                {
                    LuaFunction luaFun = _dicCacheAction[sPath][i];
                    if (luaFun != null)
                    {
                        luaFun.Call(value);
                    }
                }
                _dicCacheAction[sPath].Clear();

                //保存加载内容
                if (_dicCacheTexture2D.ContainsKey(sPath))
                {
                    _dicCacheTexture2D[sPath] = value;
                }
                else
                {
                    _dicCacheTexture2D.Add(sPath, value);
                }
            }
            else
            {
                //对所有的加载入口进行回调
                for (var i = 0; i < _dicCacheAction[sPath].Count; i++)
                {
                    LuaFunction luaFun = _dicCacheAction[sPath][i];
                    if (luaFun != null)
                    {
                        luaFun.Call(null);
                    }
                }
                _dicCacheAction[sPath].Clear();

                //保存加载内容
                if (_dicCacheTexture2D.ContainsKey(sPath))
                {
                    _dicCacheTexture2D[sPath] = null;
                }
                else
                {
                    _dicCacheTexture2D.Add(sPath, null);
                }
            }
        }
        else if (www.isDone && www.error != null)
        {
            DebugerHelper.Log("下载Texture2D错误：" + www.error, DebugerHelper.LevelType.Error);
            for (var i = 0; i < _dicCacheAction[sPath].Count; i++)
            {
                LuaFunction luaFun = _dicCacheAction[sPath][i];
                if (luaFun != null)
                {
                    luaFun.Call(null);
                }
            }
            _dicCacheAction[sPath].Clear();

            //保存加载内容
            if (_dicCacheTexture2D.ContainsKey(sPath))
            {
                _dicCacheTexture2D[sPath] = null;
            }
            else
            {
                _dicCacheTexture2D.Add(sPath, null);
            }
        }
    }

    #endregion

}
