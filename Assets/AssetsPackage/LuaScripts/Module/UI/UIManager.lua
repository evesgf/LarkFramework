--Canvas层级，需和场景中的一致
EnumUICanvas={DefaultCanvas,TopCanvas}

luaHaper=CS.LuaHapler
resManager=luaHaper.GetResManager()
xLuaBehaviour=CS.XLuaBehaviour

--require("Module/UIBase")
--require("Module/UIPage")
require("UI/UIDef")

UIManager={}
local this=UIManager
local gameObject
local transform

--UI脚本列表
local UIList={}

--UI打开的栈
local pageStack={}

--当前打开的UI
local nowPage

function this.Init()
    gameObject= CS.UnityEngine.GameObject.Find("UIManager")
    transform=gameObject.transform
    --Canva层级
    EnumUICanvas.DefaultCanvas=CS.UnityEngine.GameObject.Find("DefaultCanvas").transform
    EnumUICanvas.TopCanvas=CS.UnityEngine.GameObject.Find("TopCanvas").transform

    --加载UI脚本
    for key,value in pairs(UIDef) do
        local page=require(value)
        if type(page) == 'boolean' then     
            error('[UIManager], 找不到lua page文件:'..tostring(UIList[key])..',path: '..value)
        end
        
        UIList[value]=page:new()
    end

    print('[UIManager]'..'Init OK:'..gameObject.name)
end

--打开窗口
function this.ShwoPanel(uiDef)

    if nowPage~=nil then
        --压入栈
        table.insert( pageStack, nowPage)
        this.HideNowPanel()
    end

    nowPage=UIList[uiDef]

    if nowPage.gameObject==nil then
        --实例化UI对象
        local o = resManager:LoadPrefab(nowPage.abName,nowPage.assetName)
        local obj1= CS.UnityEngine.GameObject.Instantiate(o,nowPage.canvas)
        print(obj1)
        print(uiDef)
        xLuaBehaviour.Attach(obj1, uiDef)
        nowPage.gameObject=obj1
    end

    --执行打开操作
    nowPage:Open()

end

--关闭UI
function this.HidePanel(uiDef)
    UIList[uiDef]:Close()
end

--关闭当前UI
function this.HideNowPanel()
    nowPage:Close()
end

--返回上一个UI
function this.BackLastPage()
    nowPage:Close()
    table.remove(pageStack):Open()
end