require 'Game.ResCache'

print('GameStart from lua')
function printTest(strxx)
	print(strxx)
end

function onMonsterLoaded(prefab)
	Object.Instantiate(prefab)
end

ResCache.loadMonster('qixinpiaochong', 'onMonsterLoaded')
