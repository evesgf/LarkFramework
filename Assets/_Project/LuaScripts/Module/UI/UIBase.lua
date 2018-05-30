local _M = {}

function _M:new(path)
    return setmetatable({ assetPath = path}, { __index = _M })
end

function _M:show()
    print(self.assetPath)
end

-- function _M:hello(arg)
--     print('------------->'..self.assetPath .. ": hello in parent:" .. tostring(arg))
-- end

function XXXX()
    return '111111'
end

return  _M