ResCache = {}
ResCache.caches = {}
local caches = ResCache.caches

--cs callback
function addToCache(objAsset)
	if caches[objAsset] == nil then caches[objAsset.name] = objAsset end
end

function ResCache.loadMonster(strAsset, strCallBack)
    if caches[strAsset] == nil then
		--NOTE:monster's name is the same as bundle's file name
		local bundle = strAsset
		--tailcall addToCache
		LuaHelper.getInstance():loadFromBundle(bundle, strAsset, strCallBack)
	else
        LuaHelper.getInstance():callLuaFunc(strCallBack, caches[strAsset])
	end
end

