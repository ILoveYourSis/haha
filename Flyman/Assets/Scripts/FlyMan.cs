using UnityEngine;
using System.Collections;

public class FlyMan : NewBehaviour {

	private void Awake()
    {
        //rand start angle
        float theta_x          = Random.Range(30, 60);
        rb.mass                = 70f;
        rb.useGravity          = true;
        rb.isKinematic         = false;
        rb.angularDrag         = 0f;
        const float INIT_SPEED = 20f;
        rb.velocity            = new Vector3(Mathf.Cos(theta_x), Mathf.Sin(theta_x), 0) * INIT_SPEED;
    }
}
