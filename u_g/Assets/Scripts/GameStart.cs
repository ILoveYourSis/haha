using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class GameStart : LuaMono {
    private LuaScriptMgr _lsm;
    private void Awake()
    {
        _lsm = getLuaScriptMgr();
        _lsm.DoFile("Game.GameStart");
        Debug.Log(File.ReadAllText(GameConfig.getBundlePath() + "/qixinpiaochong.manifest.meta"));
    }

    private void Update()
    {
        _lsm.Update();
    }

    private void FixedUpdate()
    {
        _lsm.FixedUpdate();
    }
    
    private void LateUpdate()
    {
        _lsm.LateUpate();
    }
}

public class LuaHelper : LuaMono
{
    private static LuaHelper _instance = null;
    public static LuaHelper getInstance()
    {
        if(_instance == null) _instance = new GameObject("LuaHelper").AddComponent<LuaHelper>();
        return _instance;
    }
    
    public void callLuaFunc(string func, object arg)
    {
        getLuaScriptMgr().CallLuaFunction(func, arg);
    }
    /// <summary>
    /// 只加载不缓存,缓存放在lua里面
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="asset"></param>
    /// <param name="luaCallBack"></param>
    public void loadFromBundle(string bundle, string asset, string luaCallBack)
    {
        StartCoroutine(loadFromBundle2(bundle, asset, luaCallBack));
    }

    private Dictionary<string, int> _processingBunldles = new Dictionary<string, int>();
    private IEnumerator loadFromBundle2(string bundle, string asset, string luaCallBack)
    {
        while(_processingBunldles.ContainsKey(bundle)) yield return null;
        _processingBunldles.Add(bundle, 0);
        string bundleUrl = "file:///" +  GameConfig.getBundlePath() + "/" + bundle;
        WWW www;
        www = new WWW(bundleUrl);
        while(!www.isDone) yield return null;

        AssetBundle ab = www.assetBundle;
        AssetBundleRequest abr = ab.LoadAssetAsync(asset);
        while(!abr.isDone) yield return null;
        getLuaScriptMgr().CallLuaFunction("addToCache",abr.asset);
        getLuaScriptMgr().CallLuaFunction(luaCallBack, abr.asset);
        ab.Unload(true);
        _processingBunldles.Remove(bundle);
    }
}

public class InvalidUrlException: Exception
{
    public InvalidUrlException(string url)
    {
    }

}
