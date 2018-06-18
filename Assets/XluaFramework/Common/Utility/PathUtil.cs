using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 路径管理类
/// </summary>
public class PathUtil
{
    public const string BuildRootPath = "Assets/AssetsPackage/";            //打包的根路径，在打包ab包和使用模式2的时候会用到
    public const string LuaRootPath = "LuaScripts/";                        //Lua打包的根路径
    public const string ABRootPath = "AB/";                                 //AB包打包的根路径

    /// <summary>
    /// 数据盘的根目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            switch (ResManager.NowResMode)
            {

                case ResMode.Default:
                    return GetWorkPath;

                case ResMode.LocalAB:
                    if (Application.isMobilePlatform)
                    {
                        return Application.persistentDataPath + Application.productName + "/";
                    }
                    else
                    {
                        return GetWorkPath + BuildRootPath;
                    }

                case ResMode.Online:
                    if (Application.isMobilePlatform)
                    {
                        return Application.persistentDataPath + Application.productName + "/";
                    }
                    else
                    {
                        return "c://" + Application.productName + "/";
                    }

                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// 程序工作跟路径，在编辑器里是Assets/目录的上一级
    /// </summary>
    /// <returns>返回绝对路径，如：E:/Project/XLuaFramewrok/</returns>
    public static string GetWorkPath
    {
        get { return Environment.CurrentDirectory.Replace("\\", "/") + "/"; }
    }
}
