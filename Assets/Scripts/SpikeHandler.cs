using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHandler : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _impaledPosition;
    private Quaternion _impaledRotation;
    private bool _isImpaled;
    private Zombie _zombie;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _zombie = GetComponentInParent<Zombie>();
    }

    private void FixedUpdate()
    {
        if (_isImpaled)
        {
            _rigidbody.MovePosition(_impaledPosition);
            _rigidbody.MoveRotation(_impaledRotation);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Spikes>())
        {
            if (_zombie.isZombieWalking())
            {
                Vector3 force = new Vector3(0, 0, 0);
                _zombie.TriggerRagdoll(force, transform.position);
            }
            _rigidbody.isKinematic = true;
            _impaledPosition = _rigidbody.position;
            _impaledRotation = _rigidbody.rotation;
            _isImpaled = true;
            _zombie.Impale();
        }
    }
}
