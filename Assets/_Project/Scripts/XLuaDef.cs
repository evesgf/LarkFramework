using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

#if UNITY_EDITOR
public static class XLuaDef
{

    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Action),
        typeof(Action<float>),
        typeof(Action<float, float>),
        typeof(Action<GameObject>),
        typeof(UIUtil),
        typeof(Slate.Cutscene),
    };
}
#endif
