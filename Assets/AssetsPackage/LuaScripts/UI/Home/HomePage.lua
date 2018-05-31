local parent = require("Module/UI/UIPage")

local _M = {}

function _M:new()
        local obj = parent:new('ui','HomePage',EnumUICanvas.DefaultCanvas,nil,nil)
        local super_mt = getmetatable(obj)
        -- 当方法在子类中查询不到时，再去父类中去查找。
        setmetatable(_M, super_mt)
        -- 这样设置后，可以通过self.super.method(self, ...) 调用父类的已被覆盖的方法。
        --TODO:这里有命名重名问题
        obj.base = setmetatable({}, super_mt)
    return setmetatable(obj, { __index = _M })
end

-- 各个组件路径
local path_btn_Play="Title/btn_Play/Button"
local btn_Play
local path_btn_Setting="btn_box/btn_Setting/Button"
local btn_Setting
local path_btn_Exit="btn_box/btn_Exit/Button"
local btn_Exit

function Awake(obj)
    gameObject=obj
    transform=gameObject.transform

    --查找绑定
    btn_Play=transform:Find(path_btn_Play):GetComponent("Button")
    btn_Play.onClick:AddListener(_M.OnBtnPlay)
    btn_Setting=transform:Find(path_btn_Setting):GetComponent("Button")
    btn_Setting.onClick:AddListener(_M.OnBtnSetting)
    btn_Exit=transform:Find(path_btn_Exit):GetComponent("Button")
    btn_Exit.onClick:AddListener(_M.OnExit)
end

function OnDestroy()
    btn_Play.onClick:RemoveAllListeners()
    btn_Setting.onClick:RemoveAllListeners()
    btn_Exit.onClick:RemoveAllListeners()
end

function _M:OnBtnPlay()
        UIManager.ShwoPanel(UIDef.CardListPage); 
end

function _M:OnBtnSetting()

end

function _M:OnExit()
    CS.UnityEngine.Application.Quit()
end

-- 覆盖父类的方法。
function _M:Open(arg)
        -- 只能使用这种方法调用基类的方法。
        self.base.Open(self, arg)
end

return _M