local _M = {}

function _M:new(abName,assetName,canvas,gameObject,transform)
    return setmetatable(
        { 
            abName = abName or '1',
            assetName=assetName or '2',
            canvas=canvas or '3',
            gameObject=gameObject or nil,
            transform=transform or nil
        }, 
        { __index = _M })
end

function _M:show()
    print('UIBase abName:'..self.abName..' abName:'..self.assetName.." canvas:"..self.canvas)
end

function _M:hello(arg)
        print('UIBase abName:'..self.abName .. ": hello in parent:" .. tostring(arg))
end

return  _M