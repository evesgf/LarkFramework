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

local UIList={}

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
        UIList[value]=page
    end

    print('[UIManager]'..'Init OK:'..gameObject.name)
end

--打开窗口
function this.ShwoPanel(uiDef)

    nowPage=UIList[uiDef]
    local abName,assetName=nowPage:GetAssetInfo()
    local o = resManager:LoadPrefab(abName,assetName)
    local obj1= CS.UnityEngine.GameObject.Instantiate(o)
    xLuaBehaviour.Attach(obj1, uiDef)

    nowPage:show()
end

--关闭窗口
function this.HidePanel(panelName)

end

--打开OpenLoadResCheckUI
function this.OpenLoadResCheckUI()
    print('---------OpenLoadResCheckUI---------')
end