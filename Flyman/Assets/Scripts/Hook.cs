using UnityEngine;
using System.Collections;

public class Hook : NewBehaviour {

    private const float LINK_SPD = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Link link = other.GetComponent<Link>();
        if(link != null)
        {
            GameObject player  = link.getShooter();
            Rigidbody playerRB = player.GetComponent<Rigidbody>();
            //shoot player
            playerRB.velocity  = (trans.position - player.transform.position).normalized * LINK_SPD;
            Debug.Log("self destroy while come close enough, for visual effect");
            //Destroy(gameObject);
        }
    }
		
    private static GameObject _prefab;
    public static Hook genHook()
    {
        if(_prefab == null) _prefab = Resources.Load("hook") as GameObject;
        return Instantiate(_prefab).AddComponent<Hook>();
    }
}


