using System.Collections.Generic;
using UnityEngine;

[XLua.ReflectionUse]
public class AssetPool
{
    private static Dictionary<int, Dictionary<string, AssetInfo>> _dic = new Dictionary<int, Dictionary<string, AssetInfo>>();

    /// <summary>
    /// 某个资源是否存在
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="sName"></param>
    /// <returns></returns>
    private static bool IsExistByName(Dictionary<string, AssetInfo> dic, string sName)
    {
        if (dic.ContainsKey(sName))
            return true;
        return false;
    }

    /// <summary>
    /// 获取某类资源对象
    /// </summary>
    public static AssetInfo GetAsset(string sName, string sPath, int nType)
    {
        if (IsExistByType(nType))
        {
            Dictionary<string, AssetInfo> dic = _dic[nType];
            if (IsExistByName(dic, sName))
            {
                var assetInfo = dic[sName];
                if (assetInfo == null)
                {
                    dic.Remove(sName);
                }
                return assetInfo;
            }
        }
        return null;
    }

    /// <summary>
    /// 对象生成后进行缓存
    /// </summary>
    public static void AssetCache(string sName, string sPath, int nType, Object obj)
    {
        if (!IsExistByType(nType))
        {
            _dic[nType] = new Dictionary<string, AssetInfo>();
        }
        AssetInfo ai = new AssetInfo(sName, sPath, nType, obj);
        if (IsExistByName(_dic[nType], sName))
            _dic[nType][sName] = ai;
        else
            _dic[nType].Add(sName, ai);
    }

    /// <summary>
    /// 对象用完之后进行回收
    /// </summary>
    public static void AssetRecycle()
    {

    }

    /// <summary>
    /// 清理某种类型的全部资源
    /// </summary>
    /// <param name="nType"></param>
    public static void DeleteAssetByType(int nType)
    {
        if (IsExistByType(nType))
        {
            foreach (var asset in _dic[nType])
            {
                asset.Value.UnLoadAsset();
            }
            _dic[nType].Clear();
        }
    }

    /// <summary>
    /// 清理某个资源
    /// </summary>
    /// <param name="nType"></param>
    /// <param name="sName"></param>
    public static void DeleteAssetByName(int nType, string sName)
    {
        if (IsExistByType(nType))
        {
            Dictionary<string, AssetInfo> dic = _dic[nType];
            if (IsExistByName(dic, sName))
            {
                dic[sName].UnLoadAsset();
                dic.Remove(sName);
            }
        }
    }

    /// <summary>
    /// 某种类型的资源是否存在
    /// </summary>
    /// <param name="nType"></param>
    /// <returns></returns>
    public static bool IsExistByType(int nType)
    {
        if (_dic.ContainsKey(nType))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 某个资源是否存在
    /// </summary>
    /// <param name="nType"></param>
    /// <param name="sName"></param>
    /// <returns></returns>
    public static bool IsExistByName(int nType, string sName)
    {
        if (IsExistByType(nType))
        {
            Dictionary<string, AssetInfo> dic = _dic[nType];
            if (IsExistByName(dic, sName))
            {
                return true;
            }
        }
        return false;
    }
}
