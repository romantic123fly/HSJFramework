local util = require 'util'
local UnityEngine=CS.UnityEngine

--1.更改平台的生成位置
xlua.hotfix(CS.PlayerController,{
   CreatCube = function(self)
		return util.cs_generator(function()
		y = -8
			while true do
				y = y-UnityEngine.Random.Range(3,6)
				local go = UnityEngine.GameObject.Instantiate(self.cube)
				go.transform:SetParent(self.cubes.transform); 
				go.transform.position =  UnityEngine.Vector3(UnityEngine.Random.Range(-3, 3),y);
				go.transform.localScale =  UnityEngine.Vector3(UnityEngine.Random.Range(1, 3), 0.2, 1);
				coroutine.yield(CS.UnityEngine.WaitForSeconds(0.1))
			end
		end)
	end;
})

--v2.两侧添加装饰
xlua.hotfix(CS.PlayerController,{
	CreatCube = function(self)
		return util.cs_generator(function()
		y = -8
			while true do
				y = y-UnityEngine.Random.Range(3,6)
				local go = UnityEngine.GameObject.Instantiate(self.cube)
				go.transform:SetParent(self.cubes.transform); 
				go.transform.position =  UnityEngine.Vector3(UnityEngine.Random.Range(-3, 3),y);
				go.transform.localScale =  UnityEngine.Vector3(UnityEngine.Random.Range(1, 3), 0.2, 1)
				coroutine.yield(CS.UnityEngine.WaitForSeconds(1))
				
				local camera= UnityEngine.GameObject.Find('Main Camera')
				local go1 = UnityEngine.GameObject.Instantiate(self.cube)
		     	go1.transform.position =  UnityEngine.Vector3(UnityEngine.Random.Range(-8, -5),camera.transform.position.y+6,0);
				go1.transform.localScale =  UnityEngine.Vector3(0.5, 0.2, 0.5)
				go1:AddComponent(typeof(UnityEngine.Rigidbody))
			end

		end)
	end;
})


function Test()
print('saaa')
end

