using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEntry : MonoBehaviour
{
    Dictionary<GameObject, string> list = new Dictionary<GameObject, string>();

    private void Awake()
    {
        ResManager.Create().Init();
    }

    private void Update()
    {
        //同步加载资源
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var abName = "Prefabs/prefab.unity3d";
            var assetName = "Assets/_Project/Prefabs/Cube01.prefab";
            var reObj = ResManager.Instance.LoadObject<GameObject>(abName, assetName);
            var gameObject=Instantiate(reObj);
            list.Add(gameObject, abName);
        }

        //异步加载资源
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var abName = "Prefabs/prefab.unity3d";
            var assetName = "Assets/_Project/Prefabs/Cube01.prefab";
            ResManager.Instance.LoadObjectAsync<GameObject>(abName, assetName, delegate(GameObject reObj) {
                var gameObject=Instantiate(reObj);
                list.Add(gameObject, abName);
            }); 
        }

        //大资源异步加载
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResManager.Instance.LoadObjectAsync<Texture2D>("Textures/shipsize.unity3d", "Assets/_Project/Textures/ShipSize.jpg", delegate (Texture2D reObj) {
                var obj=new GameObject();
                var img=obj.AddComponent<Image>();
                img.sprite = Sprite.Create(reObj,new Rect(0,0, reObj.width, reObj.height),Vector2.zero);
            });
        }

        //卸载Prefab
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            foreach (var item in list)
            {
                Destroy(item.Key);
                ResManager.Instance.UnLoadAssetBundle(item.Value, true);
                list.Remove(item.Key);
                return;
            }
        }

        //加载场景
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResManager.Instance.LoadScene("Scenes/scene.unity3d", "Assets/_Project/Scenes/Home.unity");
        }
    }
}
