--require("Module/UI/UIManager")
--UIManager.Init()
print('XLua Init Finished!')

--打开主界面UI
--UIManager.ShwoPanel(UIDef.HomePage);
local parent = require("UI/Home/HomePage")
local c = parent:new('6','6','6')
-- 从parent继承的show方法
c:show()
-- child自己的方法。
c:hello('123456789')