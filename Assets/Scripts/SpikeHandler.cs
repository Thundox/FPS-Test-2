using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHandler : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _impaledPosition;
    private Quaternion _impaledRotation;
    private bool _isImpaled;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Spikes>())
        {
            _rigidbody.isKinematic = true;
            _impaledPosition = _rigidbody.position;
            _impaledRotation = _rigidbody.rotation;
            _isImpaled = true;
        }
    }
}
