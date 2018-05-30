using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ResBuilder {

    #region Config
    //打包路径
    public static readonly string BuildPath = Util.GetRelativePath()+ "Assets/AssetsPackage/";
    //AB包出包路径
    public static readonly string BuildABPath = BuildPath + ResManager.ABPath;
    //lua包出包路径
    public static readonly string BuildLuaPath = BuildPath + XLuaManager.luaScriptPath;
    //压缩模式
    public static readonly BuildAssetBundleOptions BuildABOptions = BuildAssetBundleOptions.None;
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
        Debug.Log("=============== Build Start ===============");

        if (Directory.Exists(BuildABPath))
        {
            Directory.Delete(BuildABPath, true);
        }
        Directory.CreateDirectory(BuildABPath);

        buildList.Clear();

        //添加打包列表
        BulidListHandler();

        //Lua打包列表
        BuildLuaHandler();

        //打包
        BuildPipeline.BuildAssetBundles(BuildABPath, buildList.ToArray(), BuildABOptions, target);

        //创建文件列表
        CreateFileList();

        AssetDatabase.Refresh();

        Debug.Log("=============== Build End ===============");
    }

    /// <summary>
    /// 添加资源到打包列表
    /// </summary>
    static void BulidListHandler()
    {
        AddBuildList("prefabs", "*.prefab", "Assets/_Project/Prefabs", false);
        AddBuildList("materials", "*.mat", "Assets/_Project/materials", true);
        AddBuildList("textures", "*.png", "Assets/_Project/Textures", true);
        AddBuildList("ui", "*.prefab", "Assets/_Project/Prefabs/UI", true);
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
        if (Directory.Exists(BuildLuaPath))
        {
            Directory.Delete(BuildLuaPath, true);
        }
        Directory.CreateDirectory(BuildLuaPath);

        //复制文件夹
        Util.CopyDirectory(Util.LuaPath + XLuaManager.luaScriptPath, BuildLuaPath);

        //todo:压缩文件
    }

    /// <summary>
    /// 生成文件列表
    /// </summary>
    [MenuItem("XLuaFramework/Create File List", false, 103)]
    static void CreateFileList()
    {
        Debug.Log("=============== Create File List Start ===============");
        //生成文件
        string filePath =BuildPath + ResManager.FileListName;
        StreamWriter sw;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        sw = File.CreateText(filePath);

        //获取文件列表
        string[] files = Directory.GetFiles(BuildPath, "*.*", SearchOption.AllDirectories);
        //以行为单位写入字符串
        foreach (var f in files)
        {
            var path =f.Replace("\\", "/");
            if (path.Equals(filePath)) continue;
            Debug.Log(path);
            sw.WriteLine(path.Substring((BuildPath).Length) + "|" + Util.Md5file(path));
        }
        sw.Close();
        sw.Dispose();

        AssetDatabase.Refresh();
        Debug.Log("=============== Create File List End ===============");
    }
}
