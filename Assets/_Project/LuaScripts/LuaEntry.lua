require("Common/CSDef")

--模块定义
require("Module/Scene/SceneManager")
require("Module/UI/UIManager")
UIManager.Init()
print('XLua Init Finished!')

--打开主界面UI
UIManager.ShwoPanel(UIDef.HomePage);