using UnityEngine;
using System.Collections;

public class NewBehaviour : MonoBehaviour {
    private Rigidbody _rb;
    public Rigidbody rb
    {
        get
        {
            if(_rb == null)
            {
                _rb = gameObject.GetComponent<Rigidbody>();
                if (_rb == null) _rb = gameObject.AddComponent<Rigidbody>();
            }
            return _rb;
        }
    }

    private Transform _trans;
    public Transform trans
    {
        get
        {
            if (_trans == null)
            {
                _trans = gameObject.GetComponent<Transform>();
            }
            return _trans;
        }
    }
}
