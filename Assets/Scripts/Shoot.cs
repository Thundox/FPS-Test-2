using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private float _maximumForce;

    [SerializeField]
    private float MaxChargeTime;

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
            LayerMask zombieLayer = LayerMask.GetMask("Default");
            if (Physics.Raycast(ray, out RaycastHit hitInfo,9999f, zombieLayer, QueryTriggerInteraction.Ignore)) 
            {
                Zombie zombie = hitInfo.collider.transform.root.GetComponent<Zombie>();
                

                if (zombie != null)
                {
                    float mouseButtonDownDuration = Time.time - _timeMouseButtonDown;
                    float forcePercentage;
                    float forceMagnitude;  //Mathf.Lerp(1, _maximumForce, forcePercentage);
                    
                    if (mouseButtonDownDuration > MaxChargeTime)
                    {
                        forcePercentage = 1;
                    }
                    else
                    {
                        forcePercentage = mouseButtonDownDuration / MaxChargeTime;
                    }

                    forceMagnitude = forcePercentage * _maximumForce;


                    /*Vector3 forceDirection = zombie.transform.position - _camera.transform.position;
                    //forceDirection.y = 1;
                    forceDirection.Normalize();
                    */
                    Vector3 force = Vector3.forward;
                    
                    zombie.TriggerRagdoll(force, hitInfo.point);

                    hitInfo.transform.GetComponent<Rigidbody>().AddForce(transform.forward * forceMagnitude, ForceMode.Impulse);
                }


            }

        }
    }
}
