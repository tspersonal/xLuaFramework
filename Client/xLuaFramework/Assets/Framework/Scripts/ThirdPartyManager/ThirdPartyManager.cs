using UnityEngine;

/// <summary>
/// 用做第三方插件的管理类
/// </summary>
[XLua.ReflectionUse]
public class ThirdPartyManager : SingletonMonoBehaviour<ThirdPartyManager>
{
    public override void DoAwake()
    {
        base.DoAwake();

        DontDestroyOnLoad(this.gameObject);
    }
}
