local parent = require("Module/UI/UIBase")

local _M = {}


function _M:new()
        local obj = parent:new()
        local super_mt = getmetatable(obj)
        -- 当方法在子类中查询不到时，再去父类中去查找。
        setmetatable(_M, super_mt)
        -- 这样设置后，可以通过self.super.method(self, ...) 调用父类的已被覆盖的方法。
        obj.super = setmetatable({}, super_mt)
    return setmetatable(obj, { __index = _M })
end

function XXXX()
        return '22222'
    end

-- 覆盖父类的方法。
-- function _M:hello()
--         -- 只能使用这种方法调用基类的方法。
--         self.super.hello(self, "super")
--         print(tostring(self.assetPath) .. ": child")
-- end

return _M