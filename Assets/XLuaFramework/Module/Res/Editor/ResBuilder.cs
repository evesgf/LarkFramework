using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ResBuilder {

    #region Config
    //打包路径
    public static readonly string BuildPath = "Assets/"+ResManager.ABPath;
    #endregion

    #region Menu
    [MenuItem("XLuaFramework/Build iPhone Resource", false, 100)]
    public static void BuildiPhoneResource()
    {
        BuildTarget target;
#if UNITY_5
        target = BuildTarget.iOS;
#else
        target = BuildTarget.iOS;
#endif
        BuildAssetResource(target);
    }

    [MenuItem("XLuaFramework/Build Android Resource", false, 101)]
    public static void BuildAndroidResource()
    {
        BuildAssetResource(BuildTarget.Android);
    }

    [MenuItem("XLuaFramework/Build Windows Resource", false, 102)]
    public static void BuildWindowsResource()
    {
        BuildAssetResource(BuildTarget.StandaloneWindows);
    }
    #endregion

    //资源列表
    private static List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();

    /// <summary>
    /// 生成ab
    /// </summary>
    /// <param name="target">目标平台</param>
    private static void BuildAssetResource(BuildTarget target)
    {
        if (Directory.Exists(BuildPath))
        {
            Directory.Delete(BuildPath, true);
        }
        Directory.CreateDirectory(BuildPath);

        buildList.Clear();

        //Lua打包列表
        BuildLuaHandler();

        //添加打包列表
        BulidListHandler();

        //打包
        BuildPipeline.BuildAssetBundles(BuildPath, buildList.ToArray(), BuildAssetBundleOptions.None, target);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 添加资源到打包列表
    /// </summary>
    static void BulidListHandler()
    {
        AddBuildList("prefabs", "*.prefab", "Assets/_Project/Prefabs", true);
        AddBuildList("materials", "*.mat", "Assets/_Project/materials", true);
        AddBuildList("textures", "*.png", "Assets/_Project/Textures", true);
    }

    /// <summary>
    /// 将资源添加到需要打包的List里
    /// </summary>
    /// <param name="bundleName">AB包名，小写</param>
    /// <param name="pattern">用于查找的扩展名</param>
    /// <param name="path">资源文件路径</param>
    /// /// <param name="subFolder">是否对子文件夹起作用</param>
    static void AddBuildList(string bundleName, string pattern, string path,bool subFolder)
    {
        //是否包含子目录中的文件
        var searchOption = subFolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        //根据扩展名查找path下的相关文件
        string[] files = Directory.GetFiles(Util.GetRelativePath()+ path, pattern, searchOption);
        if (files.Length == 0) return;

        for (int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Substring(Util.GetRelativePath().Length).Replace('\\', '/');
            Debug.Log(files[i]);
        }
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = bundleName + ResManager.ABPattern;
        build.assetNames = files;
        buildList.Add(build);
    }

    /// <summary>
    /// 处理Lua打包
    /// </summary>
    static void BuildLuaHandler()
    {
        //string streamDir = DataPath;
        //if (!Directory.Exists(streamDir)) Directory.CreateDirectory(streamDir);
    }
}
