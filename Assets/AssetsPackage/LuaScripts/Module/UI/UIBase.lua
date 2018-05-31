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

function _M:Open(arg)
    --print('OpenUI:'..self.gameObject.name)
    self.gameObject:SetActive(true)
    CS.UIUtil.ShowGUIAnim(self.gameObject)
end

function _M:Close(arg)
    --print('Close:'..self.gameObject.name)
    --CS.UIUtil.HideGUIAnim(self.gameObject)
    self.gameObject:SetActive(false)
end

return  _M