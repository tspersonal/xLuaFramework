using Debuger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

/// <summary>
/// 资源类型 Resources使用 仅作显示
/// </summary>
public enum ResType
{
    None,
    GameObject,//物体
    Text,//文本资源
    Sprite,//精灵
    Texture,//纹理
    Sound,//声音
    Effect,//特效
    Shader,//着色器
    Material,//材质
}

/// <summary>
/// 游戏当前的类型 仅作显示
/// </summary>
public enum GameType
{
    None,
    Common,//公共
    Main,//大厅
    Dp,//德州扑克
    Sh,//梭哈
    Mj,//麻将
    Sss,//十三水
    Ddz,//地主
    Nn,//牛牛
    Zjh,//炸金花
    Club,//俱乐部
}

[XLua.ReflectionUse]
public class AssetsManager : SingletonMonoBehaviour<AssetsManager>
{
    #region Resources加载资源

    /// <summary>
    /// 正在加载的列表
    /// </summary>
    private List<AssetInfo> _listLoading = new List<AssetInfo>();

    /// <summary>
    /// 正在加载的目录，使用协程
    /// </summary>
    private Dictionary<string, AssetInfo> _dicLoading = new Dictionary<string, AssetInfo>();

    //协程挂起加载
    public AssetInfo LoadAssetsAsyncByIEnumerator(string sName, string sPath, int rt, LuaFunction fun)
    {
        //正在被加载 还没加载完成
        foreach (AssetInfo item in _listLoading)
        {
            if (item.SName == sName)
            {
                item.AddListener(fun);
                return null;
            }
        }

        AssetInfo ai = AssetPool.GetAsset(sName, sPath, (int)rt);
        if (ai != null)
        {
            if (ai.ObjAsset == null)
            {
                AssetPool.DeleteAssetByName(rt, sName);
            }
            else
            {
                return ai;
            }
        }
        //都没有 先创建
        AssetInfo asset = new AssetInfo(sName, sPath, (int)rt, null);
        asset.AddListener(fun); //添加回调
        _dicLoading.Add(sName, asset); //缓存
        StartCoroutine(IeLoadAsync(asset)); //挂起
        return null;
    }

    private IEnumerator IeLoadAsync(AssetInfo asset)
    {
        //发起异步加载请求
        ResourceRequest request = asset.LoadAsync(asset.SPath);
        if (request == null)
        {
            DebugerHelper.Log("回调异常！未正常创建ResourceRequest", DebugerHelper.LevelType.Except);
        }
        else
        {
            //挂起等待加载完毕
            yield return request;
            //加载完成，判断下
            if (request.isDone)
            {
                asset.ObjAsset = request.asset;
                if (asset.ObjAsset == null)
                {
                    if (_dicLoading.ContainsKey(asset.SName))
                    {
                        _dicLoading.Remove(asset.SName);
                    }
                    DebugerHelper.Log("加载资源<" + asset.SPath + ">异常！资源未成功加载！", DebugerHelper.LevelType.Except);
                }
                else
                {
                    if (asset.ListListener != null)
                    {
                        for (int j = 0; j < asset.ListListener.Count; j++)
                        {
                            if (asset.ListListener[j] != null)
                            {
                                asset.ListListener[j].Call(asset);
                            }
                            else
                            {

                                if (_dicLoading.ContainsKey(asset.SName))
                                {
                                    _dicLoading.Remove(asset.SName);
                                }
                                DebugerHelper.Log("加载资源<" + asset.SPath + ">后，回调异常！", DebugerHelper.LevelType.Except);
                            }
                        }
                        AssetPool.AssetCache(asset.SName, asset.SPath, asset.NAssetType, asset.ObjAsset);
                        if (_dicLoading.ContainsKey(asset.SName))
                        {
                            _dicLoading.Remove(asset.SName);
                        }
                    }
                    else
                    {
                        if (_dicLoading.ContainsKey(asset.SName))
                        {
                            _dicLoading.Remove(asset.SName);
                        }
                        DebugerHelper.Log("加载资源<" + asset.SPath + ">后，无回调类型！", DebugerHelper.LevelType.Except);
                    }
                }
            }
            else
            {
                if (_dicLoading.ContainsKey(asset.SName))
                {
                    _dicLoading.Remove(asset.SName);
                }
                DebugerHelper.Log("加载资源<" + asset.SPath + ">未完成！", DebugerHelper.LevelType.Except);
            }
        }


    }

    #endregion

    #region AssetBundle加载资源

    /// <summary>
    /// 正在加载的列表
    /// </summary>
    private List<AbAssetInfo> _listAbLoading = new List<AbAssetInfo>();
    /// <summary>
    /// 正在加载的目录，使用协程
    /// </summary>
    private Dictionary<string, AbAssetInfo> _dicAbLoading = new Dictionary<string, AbAssetInfo>();

    //协程挂起加载 在Lua中调用
    public AbAssetInfo LoadAbAssetsAsyncByIEnumerator(string sAbName, string sAbPath, int rt, LuaFunction fun)
    {
        sAbPath = PathUtil.GetAssetBundleOutPath() + "/" + sAbPath;
        //正在被加载 还没加载完成
        foreach (AbAssetInfo item in _listAbLoading)
        {
            if (item.SAbName == sAbName)
            {
                item.AddListener(fun);
                return null;
            }
        }

        AbAssetInfo ai = AbAssetPool.GetAbAsset(sAbName, sAbPath, rt);
        //if (ai != null && sAbName == "prefabs/uipanel/Common/panel_second_dialog.ab")
        //{
        //    ai.UnLoadAsset();
        //    ai = null;
        //}
        if (ai != null)
        {
            if (ai.AbAsset == null)
            {
                AbAssetPool.DeleteAssetByName(rt, sAbName);
            }
            else
            {
                return ai;
            }
        }
        //都没有 先创建
        AbAssetInfo asset = new AbAssetInfo(sAbName, sAbPath, rt, null);
        asset.AddListener(fun); //添加回调
        _dicAbLoading.Add(sAbName, asset); //缓存
        StartCoroutine(IeLoadAbAsync(asset)); //挂起
        return null;
    }

    private IEnumerator IeLoadAbAsync(AbAssetInfo asset)
    {
        //发起异步加载请求
        AssetBundleCreateRequest request = asset.LoadAsync(asset.SAbName, asset.SAbPath);
        if (request == null)
        {
            DebugerHelper.Log("回调异常！未正常创建AssetBundleCreateRequest", DebugerHelper.LevelType.Except);
        }
        else
        {
            //挂起等待加载完毕
            yield return request;
            //加载完成，判断下
            if (request.isDone)
            {
                asset.AbAsset = request.assetBundle;
                if (asset.AbAsset == null)
                {
                    if (_dicAbLoading.ContainsKey(asset.SAbName))
                    {
                        _dicAbLoading.Remove(asset.SAbName);
                    }
                    DebugerHelper.Log("加载资源<" + asset.SAbName + ">异常！资源未成功加载！", DebugerHelper.LevelType.Except);
                }
                else
                {
                    if (asset.ListListener != null)
                    {
                        for (int j = 0; j < asset.ListListener.Count; j++)
                        {
                            if (asset.ListListener[j] != null)
                            {
                                asset.ListListener[j].Call(asset);
                            }
                            else
                            {
                                if (_dicAbLoading.ContainsKey(asset.SAbName))
                                {
                                    _dicAbLoading.Remove(asset.SAbName);
                                }
                                DebugerHelper.Log("加载资源<" + asset.SAbName + ">后，回调异常！", DebugerHelper.LevelType.Except);
                            }
                        }
                        AbAssetPool.AssetCache(asset.SAbName, asset.SAbName, asset.NAssetType, asset.AbAsset);
                        if (_dicAbLoading.ContainsKey(asset.SAbName))
                        {
                            _dicAbLoading.Remove(asset.SAbName);
                        }
                    }
                    else
                    {
                        if (_dicAbLoading.ContainsKey(asset.SAbName))
                        {
                            _dicAbLoading.Remove(asset.SAbName);
                        }
                        DebugerHelper.Log("加载资源<" + asset.SAbName + ">后，无回调类型！", DebugerHelper.LevelType.Except);
                    }
                }
            }
            else
            {
                if (_dicAbLoading.ContainsKey(asset.SAbName))
                {
                    _dicAbLoading.Remove(asset.SAbName);
                }
                DebugerHelper.Log("加载资源<" + asset.SAbName + ">未完成！", DebugerHelper.LevelType.Except);
            }
        }
    }
    #endregion
}
