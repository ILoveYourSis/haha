using UnityEngine;
using System.Collections;
using LuaInterface;

public class LuaMono : MonoBehaviour {
    private static LuaScriptMgr _lsm = null;
    protected LuaScriptMgr getLuaScriptMgr()
    {
        if(_lsm == null)
        {
            _lsm = new LuaScriptMgr();
            _lsm.Start();
        }
        return _lsm;
    }
}
