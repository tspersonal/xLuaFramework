using UnityEngine.SceneManagement;

[XLua.ReflectionUse]
public class LuaScenesComplete : ExtendMonoBehaviour
{
    public override void DoStart()
    {
        base.DoStart();
        if (LuaFunctionManager.OnLoadSceneComplete != null)
        {
            LuaFunctionManager.OnLoadSceneComplete(SceneManager.GetActiveScene().name);
        }
    }
}
