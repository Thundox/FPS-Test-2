using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private float _maximumForce;

    [SerializeField]
    private float _maximumForceTime;

    private float _timeMouseButtonDown;

    private Camera _camera;

    // Start is called before the first frame update
    void Awake()
    {
       _camera = GetComponent<Camera>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _timeMouseButtonDown = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo)) 
            {
                Zombie zombie = hitInfo.collider.GetComponentInParent<Zombie>();

                if (zombie != null)
                {
                    float mouseButtonDownDuration = Time.time - _timeMouseButtonDown;
                    float forcePercentage = mouseButtonDownDuration / _maximumForce;
                    float forceMagnitude = _maximumForce; //Mathf.Lerp(1, _maximumForce, forcePercentage);

                    Vector3 forceDirection = zombie.transform.position - _camera.transform.position;
                    //forceDirection.y = 1;
                    forceDirection.Normalize();

                    Vector3 force = forceMagnitude * forceDirection;

                    zombie.TriggerRagdoll(force, hitInfo.point);
                }


            }

        }
    }
}
