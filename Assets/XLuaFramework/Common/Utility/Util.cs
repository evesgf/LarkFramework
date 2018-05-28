using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static string DataPath
    {
        get
        {
            string appName = Application.productName;
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + appName + "/";
            }
            else
            {
                return GetRelativePath();
            }
        }
    }

    /// <summary>
    /// 获取绝对路径
    /// </summary>
    /// <returns></returns>
    public static string GetRelativePath()
    {
        //编辑器模式
        if (Application.isEditor)
            return System.Environment.CurrentDirectory.Replace("\\", "/") + "/";
        return null;
    }
}
