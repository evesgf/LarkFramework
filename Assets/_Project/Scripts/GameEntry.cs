using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            var prefab1 = ResManager.Instance.LoadPrefab<GameObject>("prefabs", "Cube01");
            var obj1 = GameObject.Instantiate(prefab1);
            list.Add(obj1, "prefabs");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResManager.Instance.LoadPrefabAsync<GameObject>("prefabs", "Cube02", delegate (GameObject prefab2) {
                var obj2=GameObject.Instantiate(prefab2);
                list.Add(obj2, "prefabs");
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
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

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            foreach (var item in list)
            {
                Destroy(item.Key);
                ResManager.Instance.UnLoadAssetBundle(item.Value + ResManager.ABPattern, true);
                list.Remove(item.Key);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ResManager.Instance.UnloadUnusedAssets();
        }
    }
}
