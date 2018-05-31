local parent = require("Module/UI/UIPage")

local _M = {}

function _M:new()
        local obj = parent:new('ui','CardListPage',EnumUICanvas.DefaultCanvas,nil,nil)
        local super_mt = getmetatable(obj)
        -- 当方法在子类中查询不到时，再去父类中去查找。
        setmetatable(_M, super_mt)
        -- 这样设置后，可以通过self.super.method(self, ...) 调用父类的已被覆盖的方法。
        --TODO:这里有命名重名问题
        obj.base = setmetatable({}, super_mt)
    return setmetatable(obj, { __index = _M })
end

-- 各个组件路径
local path_btn_Back="btn_Box/btn_Home/Button"
local btn_Back

function Awake(obj)
    gameObject=obj
    transform=gameObject.transform

    --查找绑定
    btn_Back=transform:Find(path_btn_Back):GetComponent("Button")
    btn_Back.onClick:AddListener(_M.OnBack)
end

function OnDestroy()
    btn_Back.onClick:RemoveAllListeners()
end

function _M:OnBack()
        UIManager.ShwoPanel(UIDef.HomePage); 
end

-- 覆盖父类的方法。
function _M:Open(arg)
        -- 只能使用这种方法调用基类的方法。
        self.base.Open(self, arg)
end

return _M