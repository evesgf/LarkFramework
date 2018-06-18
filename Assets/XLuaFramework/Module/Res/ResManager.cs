using LarkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ResManager:SingletonMono<ResManager> {

    #region Config
    //AB包出包路径
    public static readonly string ABPath = "AB/";
    //manifestName，默认为AB包文件夹名
    public static readonly string ManifestName = "AB";
    //AB包扩展名
    public static readonly string ABPattern = ".unity3d";
    //文件列表,存储AB包MD5
    public static readonly string FileListName = "FileList.txt";

    //更新模式
    public static readonly bool UpdaeMode = false;

#if UNITY_STANDALONE
    //PC平台资源更新地址
    public static readonly string UpdateAddress = "http://ab.evesgf.com/PC/";
#elif UNITY_IPHONE
    //IOS平台资源更新地址
    public static readonly string UpdateAddress = "http://ab.evesgf.com/IOS/";
#elif UNITY_ANDROID
    //Android平台资源更新地址
    public static readonly string UpdateAddress = "http://ab.evesgf.com/Android/";
#endif
    #endregion

    //总的Manifest，用于查询依赖
    private AssetBundleManifest m_assetBundleManifest;
    //依赖统计
    private Dictionary<string, AssetBundleInfo> m_loadedAssetBundles = new Dictionary<string, AssetBundleInfo>();

    private ResCheckPage resCheckPage;

    //资源引用相关类
    public class AssetBundleInfo
    {
        public AssetBundle m_AssetBundle;       //AB包资源
        public int m_ReferencedCount;           //引用计数

        public AssetBundleInfo(AssetBundle assetBundle)
        {
            m_AssetBundle = assetBundle;
            m_ReferencedCount = 1;              
        }
    }


    /// <summary>
    /// Manager初始化
    /// </summary>
    /// <param name="action">初始化完成后的调用</param>
    public void Init(Action initOK = null)
    {
        StartCoroutine(OnInit(initOK));
    }

    IEnumerator OnInit(Action initOK=null)
    {
        //从resources中读UI
        var obj = Resources.Load<GameObject>("ResCheckPage");
        resCheckPage = Instantiate(obj, GameObject.Find("DefaultCanvas").transform).GetComponent<ResCheckPage>();
        resCheckPage.SetSliderValue(0);
        resCheckPage.SetSliderInfo("正在检查更新");

        yield return StartCoroutine(resCheckPage.Open());

        //检查本地资源文件
        if (UpdaeMode)
        {
            yield return StartCoroutine(CheckResource());
        }

        m_loadedAssetBundles = new Dictionary<string, AssetBundleInfo>();

        //加载assetBundleManifest文件    
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(Util.DataPath + ABPath + ManifestName);
        if (manifestBundle != null)
        {
            m_assetBundleManifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            //释放包
            manifestBundle.Unload(false);
            manifestBundle = null;

            resCheckPage.SetSliderInfo("游戏初始化完成");
            Debug.Log("=================== Init Finished ===================");
            StartCoroutine(resCheckPage.Hide(delegate {
                Destroy(resCheckPage.gameObject);

                if (initOK != null)
                {
                    //初始化完成回调
                    initOK.Invoke();
                }
            }));
        }
        else
        {
            Debug.LogError("[ResManager]Init Error! manifestBundle is nil!");
        }
    }
#region 资源更新相关
    /// <summary>
    /// 检查本地资源是否存在
    /// </summary>
    IEnumerator CheckResource()
    {
        Debug.Log("=============== CheckResource Start ===============");
        if (!Directory.Exists(Util.DataPath))
        {
            //程序首次安装
            Directory.CreateDirectory(Util.DataPath);
        }

        resCheckPage.SetSliderValue(0);
        resCheckPage.SetSliderInfo("正在下载资源列表");

        //下载资源列表
        yield return StartCoroutine(DownloadFile(UpdateAddress + FileListName, Util.DataPath + FileListName));

        //逐行读取资源列表
        Dictionary<string, string> files = new Dictionary<string, string>();
        StreamReader sr = new StreamReader(Util.DataPath+ FileListName);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            //校验资源
            var info = line.Split('|');
            var filePath = info[0];
            var fileMd5 = info[1];

            files.Add(filePath, fileMd5);
        }
        if (sr != null) sr.Close();

        int i = 0;
        foreach (KeyValuePair<string, string> pair in files)
        {
            i++;

            resCheckPage.SetSliderValue(i/ files.Count);
            resCheckPage.SetSliderInfo("正在检查第" + i + "个文件");

            if (File.Exists(Util.DataPath + pair.Key))
            {
                if (Util.Md5file(Util.DataPath + pair.Key).Equals(pair.Value)) continue;
                File.Delete(Util.DataPath + pair.Key);
            }

            //下载资源
            yield return StartCoroutine(DownloadFile(UpdateAddress + pair.Key, Util.DataPath + pair.Key));
        }

        resCheckPage.SetSliderInfo("资源检查完成");
        Debug.Log("=============== CheckResource End ===============");
    }

    /// <summary>
    /// 文件下载
    /// https://docs.unity3d.com/Manual/UnityWebRequest-CreatingDownloadHandlers.html
    /// IIS服务器设置MIME类型：application/octet-stream .unity3d
    /// 无后缀则设置扩展名为.（点）：application/octet-stream .
    /// </summary>
    /// <param name="url"></param>
    /// <param name="savePath"></param>
    /// <returns></returns>
    IEnumerator DownloadFile(string url,string savePath)
    {
        var www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.Send();

        if (www.isError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //retrieve results as binary data
            byte[] results = www.downloadHandler.data;

            //创建目录
            var saveDir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }

            FileStream fs = new FileStream(savePath, FileMode.Create);
            fs.Write(results, 0, results.Length);
            fs.Close();
            fs.Dispose();
        }
    }
#endregion

#region 资源加载相关
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="abName">ab包名</param>
    /// <param name="assetName">加载的资源名</param>
    /// <returns></returns>
    public GameObject LoadPrefab(string abName, string assetName)
    {
        var ab = LoadAssets(abName+ABPattern);
        var prefab = ab.LoadAsset<GameObject>(assetName);
        if (prefab == null)
        {
            Debug.LogError("[ResManager]Failed to load asset:"+ abName+":"+ assetName);
            return null;
        }
        return prefab;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="abName">ab包名</param>
    /// <param name="assetName">加载的资源名</param>
    /// <returns></returns>
    public void LoadPrefabAsync<T>(string abName, string assetName,Action<T> action=null) where T : UnityEngine.Object
    {
        LoadAssetsAsync(abName+ABPattern,delegate(AssetBundle ab) {
            var prefab = ab.LoadAsset<T>(assetName);
            if (prefab == null)
            {
                Debug.LogError("[ResManager]Failed to load asset:" + abName + ":" + assetName);
            }
            if (action != null)
            {
                action(prefab);
            }
        });

    }

    /// <summary>
    /// 同步加载AB包
    /// </summary>
    /// <param name="abName">ab包名,如ab.unity3d</param>
    /// <returns>返回加载的资源</returns>
    public AssetBundle LoadAssets(string abName)
    {
        var path = Util.DataPath + ABPath + abName;

        AssetBundleInfo bundleInfo = null;
        //检查是否已经加载
        if (m_loadedAssetBundles.TryGetValue(path, out bundleInfo))
        {
            bundleInfo.m_ReferencedCount++;
        }
        else
        {
            //加载依赖
            string[] dependencies = m_assetBundleManifest.GetAllDependencies(abName);
            foreach (var d in dependencies)
            {
                var dPath = Util.DataPath + ABPath + d;
                AssetBundleInfo dBundleInfo = null;
                //检查是否已经加载
                if (m_loadedAssetBundles.TryGetValue(dPath, out dBundleInfo))
                {
                    dBundleInfo.m_ReferencedCount++;
                }
                else
                {
                    var dep = AssetBundle.LoadFromFile(dPath);
                    if (dep == null)
                    {
                        Debug.LogError("[ResManager]Failed to load AssetBundle:" + dPath);
                        return null;
                    }
                    dBundleInfo = new AssetBundleInfo(dep);
                    m_loadedAssetBundles.Add(dPath, dBundleInfo);
                }
            }

            //加载资源本体
            var loadAB = AssetBundle.LoadFromFile(path);
            if (loadAB == null)
            {
                Debug.LogError("[ResManager]Failed to load AssetBundle:"+path);
                return null;
            }
            bundleInfo=new AssetBundleInfo(loadAB);
            m_loadedAssetBundles.Add(path, bundleInfo);
        }

        Debug.Log("[ResManger]LoadAssets:" + path);
        return bundleInfo.m_AssetBundle;
    }

    /// <summary>
    /// 异步加载AB包
    /// </summary>
    /// <param name="abName">ab包名,如ab.unity3d</param>
    /// <param name="action">加载后的回调</param>
    /// <returns></returns>
    public void LoadAssetsAsync(string abName, Action<AssetBundle> action=null)
    {
        StartCoroutine(OnLoadAssetsAsync(abName, action));
    }

    IEnumerator OnLoadAssetsAsync(string abName,Action<AssetBundle> action=null)
    {
        var path = Util.DataPath + ABPath + abName;

        AssetBundleInfo bundleInfo = null;
        if (m_loadedAssetBundles.TryGetValue(path, out bundleInfo))
        {
            bundleInfo.m_ReferencedCount++;
        }
        else
        {
            //加载依赖
            string[] dependencies = m_assetBundleManifest.GetAllDependencies(abName);
            foreach (var d in dependencies)
            {
                var dPath = Util.DataPath + ABPath + d;
                AssetBundleInfo dBundleInfo = null;
                //检查是否已经加载
                if (m_loadedAssetBundles.TryGetValue(dPath, out dBundleInfo))
                {
                    dBundleInfo.m_ReferencedCount++;
                }
                else
                {
                    var dep = AssetBundle.LoadFromFileAsync(dPath);
                    yield return dep;
                    dBundleInfo = new AssetBundleInfo(dep.assetBundle);
                    if (dBundleInfo.m_AssetBundle == null)
                    {
                        Debug.LogError("[ResManager]Failed to load AssetBundle:" + dPath);
                    }
                    
                    m_loadedAssetBundles.Add(dPath, dBundleInfo);
                }
            }

            //加载本体
            var loadAB = AssetBundle.LoadFromFileAsync(path);
            yield return loadAB;

            bundleInfo = new AssetBundleInfo(loadAB.assetBundle);
            if (bundleInfo.m_AssetBundle == null)
            {
                Debug.LogError("[ResManager]Failed to load AssetBundle:"+path);
                yield break;
            }
            m_loadedAssetBundles.Add(path, bundleInfo);
        }

        if (action != null)
        {
            action.Invoke(bundleInfo.m_AssetBundle);
        }
    }

    /// <summary>
    /// 卸载AB包
    /// </summary>
    /// <param name="abName">ab包名,如ab.unity3d</param>
    /// <param name="isThorough">是否强制清除</param>
    public void UnLoadAssetBundle(string abName, bool isThorough = false)
    {
        var path = Util.DataPath + ABPath + abName;
        AssetBundleInfo bundleInfo = null;
        if (m_loadedAssetBundles.TryGetValue(path, out bundleInfo))
        {
            //卸载依赖
            //string[] dependencies = m_assetBundleManifest.GetAllDependencies(abName);
            //foreach (var d in dependencies)
            //{
            //    UnLoadAssetBundle(d);
            //}

            //卸载本体
            if (--bundleInfo.m_ReferencedCount <= 0)
            {
                bundleInfo.m_AssetBundle.Unload(isThorough);

                m_loadedAssetBundles.Remove(path);

                Debug.Log("[ResManager]has been unloaded successfully:" + path);
            }
            else
            {
                Debug.Log("[ResManager]"+ bundleInfo.m_ReferencedCount);
            }
        }
    }

    /// <summary>
    /// 全局弱卸载，回收无引用Asset
    /// </summary>
    public void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }
#endregion
}
