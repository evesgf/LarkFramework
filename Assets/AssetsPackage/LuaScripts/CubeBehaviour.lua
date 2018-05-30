local speed = 10

local gameObject

function Awake(obj)
    gameObject=obj
    print(gameObject.name..'----Awake')
end

function Start()
    print(gameObject.name..'----Start')
end

function OnEnable()
    print(gameObject.name..'----OnEnable')
end

function Update()

    local r = CS.UnityEngine.Vector3.up * CS.UnityEngine.Time.deltaTime * speed
	self.transform:Rotate(r)
end

function FixedUpdate()
    print(gameObject.name..'----FixedUpdate')
end

function LatedUpdate()
    print(gameObject.name..'----LatedUpdate')
end

function OnDisable()
    print(gameObject.name..'----OnDisable')
end

function OnDestroy()
    print(gameObject.name..'----OnDestroy')
end