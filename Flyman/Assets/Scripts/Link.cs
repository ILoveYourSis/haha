using UnityEngine;
using System.Collections;

public class Link : MonoBehaviour {

    private GameObject _shooter;
    public GameObject getShooter() { return _shooter; }
    /// <summary>
    /// shooter is flyman
    /// </summary>
    /// <param name="flyMan"></param>
    /// <param name="targetDirX"></param>
    /// <param name="targetDirY"></param>
    public void shoot(GameObject shooter, float targetDirX, float targetDirY)
    {
        _shooter = shooter;
    }

    private static GameObject _linkPrefab;
    public static Link genLink()
    {
        if(_linkPrefab == null) _linkPrefab = Resources.Load("link") as GameObject;
        return Instantiate(_linkPrefab).AddComponent<Link>();
    }

}
