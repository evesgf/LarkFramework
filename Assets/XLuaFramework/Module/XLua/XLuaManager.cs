using LarkFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;

public class XLuaManager : SingletonMono<XLuaManager>
{
    #region Config
    //Lua脚本的位置
    public static readonly string luaScriptPath = "LuaScripts/";
    #endregion

    public LuaEnv luaEnv;

    public void Init()
    {
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require'LuaEntry'");
    }

    /// <summary>
    /// 自定义Load
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public byte[] MyLoader(ref string fileName)
    {
        string path = Util.LuaPath + luaScriptPath + fileName + ".lua";
        return Encoding.UTF8.GetBytes(File.ReadAllText(path));
    }

    private float lastGCTime = 0;
    private void Update()
    {
        if (Time.time - lastGCTime > 1f)
        {
            luaEnv.Tick();
            lastGCTime = Time.time;
        }
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }
}
