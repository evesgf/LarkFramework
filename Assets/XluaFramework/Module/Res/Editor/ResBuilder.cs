using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 资源打包类
/// </summary>
public class ResBuilder {

    public static BuildTarget nowBuildTarget;           //当前的打包平台

    #region Menu
    [MenuItem("XLuaFramework/Build iPhone Resource", false, 100)]
    public static void BuildiPhoneResource()
    {
        nowBuildTarget = BuildTarget.iOS;
        BuildAssetResource();
    }

    [MenuItem("XLuaFramework/Build Android Resource", false, 101)]
    public static void BuildAndroidResource()
    {
        nowBuildTarget = BuildTarget.Android;
        BuildAssetResource();
    }

    [MenuItem("XLuaFramework/Build Windows Resource", false, 102)]
    public static void BuildWindowsResource()
    {
        nowBuildTarget = BuildTarget.StandaloneWindows;
        BuildAssetResource();
    }
    #endregion

    /// <summary>
    /// 打包流程
    /// </summary>
    /// <param name="target"></param>
    private static void BuildAssetResource()
    {
        Debug.Log("=============== Build " + nowBuildTarget.ToString() + " Start =============== ");

        BuildABHandler();           //AB文件打包

        AssetDatabase.Refresh();
        Debug.Log("=============== Build " + nowBuildTarget.ToString() + " Finished ============= ");
    }

    /// <summary>
    /// AB文件打包
    /// </summary>
    private static void BuildABHandler()
    {
        var buildABPath = PathUtil.GetWorkPath + PathUtil.BuildRootPath + PathUtil.ABRootPath;

        //清空文件夹
        if (Directory.Exists(buildABPath)) Directory.Delete(buildABPath, true);
        Directory.CreateDirectory(buildABPath);

        //打包列表
        AssetBundleBuild[] buildMap = new AssetBundleBuild[5];

        //贴图
        buildMap[0].assetBundleName = "Textures/texture.unity3d";
        buildMap[0].assetNames = new string[] { "Assets/_Project/Textures/t1.png" };

        //材质
        buildMap[1].assetBundleName = "Materials/materials.unity3d";
        buildMap[1].assetNames = new string[] { "Assets/_Project/Materials/m1.mat" };

        //预置件
        buildMap[2].assetBundleName = "Prefabs/prefab.unity3d";
        buildMap[2].assetNames = new string[] { "Assets/_Project/Prefabs/Cube01.prefab" };

        //场景打包
        buildMap[3].assetBundleName = "Scenes/scene.unity3d";
        buildMap[3].assetNames = new string[] { "Assets/_Project/Scenes/Home.unity" };

        //测试加载的大图
        buildMap[4].assetBundleName = "Textures/shipsize.unity3d";
        buildMap[4].assetNames = new string[] { "Assets/_Project/Textures/ShipSize.jpg" };


        BuildPipeline.BuildAssetBundles(buildABPath, buildMap, BuildAssetBundleOptions.None, nowBuildTarget);
    }
}
