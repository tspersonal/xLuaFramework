using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;

[XLua.ReflectionUse]
public class LuaManager : SingletonMonoBehaviour<LuaManager>
{

    public static string PUBLIC_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQDRl6Ip0MKk7w2IJOvObfjHu67C/xiojHukwaYToJTWR202JMUAXSOw3GzimjoFzV3fqf8AMOJ/oHGcfGzGXntlti1q89OQh89uxM4Ro4/JEhcwxdt8nz2dDFXarAUu9oPuAJcotVVgX/9YPyIO3nt0zxRtHmjIHNdOSfuQp94Oug==";

    public static LuaEnv luaEnv = new LuaEnv();
    //保存已经加载过的Lua脚本
    Dictionary<string, byte[]> dictNameToLua = new Dictionary<string, byte[]>();

    public override void DoAwake()
    {
        base.DoAwake();

        luaEnv.AddLoader(CustomLoader);
        RequireLuaBase();
        new CSharpCallLuaFunc();
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        luaEnv.GC();
    }

    /// <summary>
    /// 加载初始化Lua的脚本 在此脚本中加载其它Lua脚本
    /// </summary>
    private void RequireLuaBase()
    {
        luaEnv.DoString(@"require 'LuaRequires'");
    }

    //自定义Loader
    byte[] CustomLoader(ref string fileName)
    {
        if (dictNameToLua.ContainsKey(fileName))
        {
            return dictNameToLua[fileName];
        }
        //获取lua所在的目录
        string luaPath = "";
        if (Application.platform == RuntimePlatform.WindowsEditor && LocalData.LoadAssetsType == (int)InitGame.LoadAssetsType.Resources)
        {
            luaPath = Application.dataPath + "/Lua/" + fileName.Replace(".", "/") + ".lua.txt";
        }
        else
        {
            luaPath = PathUtil.GetAssetBundleOutPath() + "/Lua/" + fileName.Replace(".", "/") + ".lua.txt";
        }
        //luaPath = PathUtil.GetAssetBundleOutPath() + "/Lua/" + fileName.Replace(".", "/") + ".lua";
        //Debuger.Log(luaPath);
        if (File.Exists(luaPath))
        {
            string luaScript = File.ReadAllText(luaPath);
            byte[] bytes = ASCIIEncoding.UTF8.GetBytes(luaScript);
            dictNameToLua.Add(fileName, bytes);
            return bytes;
            //return File.ReadAllBytes(luaPath);
        }
        else
        {
            return null;
        }
        //return ProcessDir(new DirectoryInfo(luaPath), fileName);
    }
}
