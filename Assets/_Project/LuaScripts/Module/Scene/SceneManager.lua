require("Scene/SceneDef")

SceneManager={}
local this=SceneManager

--Scene脚本列表
local SceneList={}

--当前打开的Scene
local nowScene

function this.Init()
    --加载Scene脚本
    for key,value in pairs(SceneDef) do
        local scene=require(value)
        if type(scene) == 'boolean' then     
            error('[SceneManager], 找不到scene文件:'..tostring(SceneList[key])..',path: '..value)
        end
        
        SceneList[value]=scene
    end

    print('[SceneManager] Init OK')
end

--打开窗口
function this.SwitchScene(SceneDef)
    nowScene=SceneList[SceneDef]

    --执行打开操作
    nowScene:Load()

end