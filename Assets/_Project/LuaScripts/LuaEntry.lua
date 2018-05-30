require("Module/UI/UIManager")
UIManager.Init()
print('XLua Init Finished!')

--注意此处调用要用：号
-- local o = resManager:LoadPrefab("prefabs","Cube01")
-- local obj1= CS.UnityEngine.GameObject.Instantiate(o)
-- xLuaBehaviour.Attach(obj1, "CubeBehaviour")

UIManager.ShwoPanel(UIDef.HomePage);