using UnityEngine;
using System.Collections;

public class SceneMgr : MonoBehaviour {

    private GameObject _flyMan;
	// Use this for initialization
	void Start () {
        GameObject prefab = Resources.Load("flyman") as GameObject;
        _flyMan = Instantiate(prefab);
        _flyMan.AddComponent<FlyMan>();
        //init Cam
        Camera.main.gameObject.AddComponent<CamCtrller>().setTarget(_flyMan.transform);
	}
}
