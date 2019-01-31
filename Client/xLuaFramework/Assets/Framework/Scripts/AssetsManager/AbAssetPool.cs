using System.Collections.Generic;
using UnityEngine;

[XLua.ReflectionUse]
public class AbAssetPool
{
    private static Dictionary<int, Dictionary<string, AbAssetInfo>> _dic = new Dictionary<int, Dictionary<string, AbAssetInfo>>();

    /// <summary>
    /// 某个资源是否存在
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="sAbName"></param>
    /// <returns></returns>
    private static bool IsExistByName(Dictionary<string, AbAssetInfo> dic, string sAbName)
    {
        if (dic.ContainsKey(sAbName))
            return true;
        return false;
    }

    /// <summary>
    /// 获取某类资源对象
    /// </summary>
    public static AbAssetInfo GetAbAsset(string sAbName, string sAbPath, int nType)
    {
        if (IsExistByType(nType))
        {
            Dictionary<string, AbAssetInfo> dic = _dic[nType];
            if (IsExistByName(dic, sAbName))
            {
                var assetInfo = dic[sAbName];
                if (assetInfo == null)
                {
                    dic.Remove(sAbName);
                }
                return assetInfo;
            }
        }
        return null;
    }

    /// <summary>
    /// 对象生成后进行缓存
    /// </summary>
    public static void AssetCache(string sAbName, string sAbPath, int nType, AssetBundle abAsset)
    {
        if (!IsExistByType(nType))
        {
            _dic[nType] = new Dictionary<string, AbAssetInfo>();
        }
        AbAssetInfo ai = new AbAssetInfo(sAbName, sAbPath, nType, abAsset);
        if (IsExistByName(_dic[nType], sAbName))
            _dic[nType][sAbName] = ai;
        else
            _dic[nType].Add(sAbName, ai);
    }

    /// <summary>
    /// 对象用完之后进行回收
    /// </summary>
    public static void AssetRecycle()
    {

    }

    /// <summary>
    /// 删除某种类型的全部资源
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
    /// <param name="sAbName"></param>
    public static void DeleteAssetByName(int nType, string sAbName)
    {
        if (IsExistByType(nType))
        {
            Dictionary<string, AbAssetInfo> dic = _dic[nType];
            if (IsExistByName(dic, sAbName))
            {
                dic[sAbName].UnLoadAsset();
                dic.Remove(sAbName);
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
            Dictionary<string, AbAssetInfo> dic = _dic[nType];
            if (IsExistByName(dic, sName))
            {
                return true;
            }
        }
        return false;
    }
}
