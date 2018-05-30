using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class XLuaBehaviour : MonoBehaviour {

    public string luaScriptPath;                //Lua脚本的路径

    private LuaTable scriptEnv;                 //脚本的表

    //生命周期回调
    private Action luaStart;
    private Action luaOnEnable;
    private Action luaUpdate;
    private Action luaFixedUpdate;
    private Action luaLateUpdate;
    private Action luaOnDisable;
    private Action luaDestroy;

    public static XLuaBehaviour Attach(GameObject attachObj, string luaScriptPath)
    {
        foreach (var c in attachObj.GetComponents<XLuaBehaviour>())
        {
            if (c.luaScriptPath.Equals(luaScriptPath)) return c;
        }

        var behaviour = attachObj.AddComponent<XLuaBehaviour>();
        behaviour.luaScriptPath = Util.LuaPath + XLuaManager.luaScriptPath+luaScriptPath+".lua";
        //初始化
        behaviour.Init(attachObj);

        return behaviour;
    }

    public void Init(GameObject obj)
    {
        scriptEnv = XLuaManager.Instance.luaEnv.NewTable();
        LuaTable meta= XLuaManager.Instance.luaEnv.NewTable();
        meta.Set("__index", XLuaManager.Instance.luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        scriptEnv.Set("self", this);

        XLuaManager.Instance.luaEnv.DoString(File.ReadAllText(luaScriptPath, Encoding.UTF8), "[XLuaBehaviour]", scriptEnv);

        Action<GameObject> luaAwake = scriptEnv.Get<Action<GameObject>>("Awake");
        scriptEnv.Get("Start", out luaStart);
        scriptEnv.Get("OnEnable", out luaOnEnable);
        scriptEnv.Get("Update", out luaUpdate);
        scriptEnv.Get("FixedUpdate", out luaFixedUpdate);
        scriptEnv.Get("LatedUpdate", out luaLateUpdate);
        scriptEnv.Get("OnDisable", out luaOnDisable);
        scriptEnv.Get("OnDestroy", out luaDestroy);

        if (luaAwake != null)
        {
            luaAwake(gameObject);
        }
    }

    #region 生命周期
    void Start()
    {
        if (luaStart != null)
        {
            luaStart();
        }
    }

    void OnEnable()
    {
        if (luaOnEnable != null)
        {
            luaOnEnable();
        }
    }

    void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }
    }

    void FixedUpdate()
    {
        if (luaFixedUpdate != null)
        {
            luaFixedUpdate();
        }
    }

    void LateUpdate()
    {
        if (luaLateUpdate != null)
        {
            luaLateUpdate();
        }
    }

    void OnDisable()
    {
        if (luaOnDisable != null)
        {
            luaOnDisable();
        }
    }

    void OnDestroy()
    {
        if (luaDestroy != null)
        {
            luaDestroy();
        }
        luaDestroy = null;
        luaUpdate = null;
        luaStart = null;
        scriptEnv.Dispose();
    }
    #endregion
}

#if UNITY_EDITOR
public static class XLuaBehaviourExporter
{
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Action),
        typeof(Action<float>),
        typeof(Action<float, float>),
        typeof(Action<GameObject>),
    };
}
#endif
