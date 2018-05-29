using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEntry : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ResManager.Create().Init();
    }

    Dictionary<GameObject, string> list = new Dictionary<GameObject, string>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateCube1();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateCube2();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UnAsset1();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UnAsset2();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ResManager.Instance.UnloadUnusedAssets();
        }
    }

    public void CreateCube1()
    {
        var prefab1 = ResManager.Instance.LoadPrefab<GameObject>("prefabs", "Cube01");
        var obj1 = GameObject.Instantiate(prefab1);
        list.Add(obj1, "prefabs");
    }

    public void CreateCube2()
    {
        ResManager.Instance.LoadPrefabAsync<GameObject>("prefabs", "Cube02", delegate (GameObject prefab2) {
            var obj2 = GameObject.Instantiate(prefab2);
            list.Add(obj2, "prefabs");
        });
    }

    public void UnAsset1()
    {
        if (list.Count > 0)
        {
            foreach (var item in list)
            {
                Destroy(item.Key);
                ResManager.Instance.UnLoadAssetBundle(item.Value + ResManager.ABPattern);
                list.Remove(item.Key);
                return;
            }
        }
    }

    public void UnAsset2()
    {
        if (list.Count > 0)
        {
            foreach (var item in list)
            {
                Destroy(item.Key);
                ResManager.Instance.UnLoadAssetBundle(item.Value + ResManager.ABPattern, true);
                list.Remove(item.Key);
                return;
            }
        }
    }
}
