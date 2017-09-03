using UnityEngine;
using System.Collections;

public class CamCtrller : NewBehaviour {

    private Transform _target;

    public void setTarget(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate()
    {
        if (_target == null) return;
        Vector3 targetPos = new Vector3(_target.position.x, trans.position.y, trans.position.z);
        trans.position = Vector3.Slerp(trans.position, targetPos, 0.5f);
    }
}
