using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MsgMgr
{
    /// <summary>
    /// 事件到侦听
    /// </summary>
    private static Dictionary<System.Type, List<Action<Msg>>> _typeToListeners = new Dictionary<System.Type, List<Action<Msg>>> ();
    public static void addListener(System.Type msgType, Action<Msg> listener)
    {
        getListeners(msgType).Add(listener);
        if(!_listenerToMsgType.ContainsKey(listener)) _listenerToMsgType.Add(listener, msgType);
    }

    /// <summary>
    /// 侦听到事件
    /// </summary>
    private static Dictionary<Action<Msg>, System.Type> _listenerToMsgType = new Dictionary<Action<Msg>, Type>();
    public static void rmListener(Action<Msg> listener)
    {
        if(!_listenerToMsgType.ContainsKey(listener)) return;;
        System.Type msgType = _listenerToMsgType[listener];
         getListeners(msgType).Remove(listener);
    }

    private static List<Action<Msg>> getListeners(System.Type msgType)
    {
        if(_typeToListeners.ContainsKey(msgType)) return _typeToListeners[msgType];
        else
        {
            List<Action<Msg>> newList = new List<Action<Msg>>();
            _typeToListeners.Add(msgType, newList);
            return newList;
        }
    }

    public static void dispatchMsg(Msg msg)
    {
        List<Action<Msg>> listeners = getListeners(msg.GetType());
        foreach(Action<Msg> listener in listeners)
            listener(msg);
    }
}
