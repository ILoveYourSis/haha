using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
    private void Start()
    {
        new GameObject("SceneManager").AddComponent<SceneMgr>();
        new GameObject("InputController").AddComponent<InputCtrller>();
    }
}
