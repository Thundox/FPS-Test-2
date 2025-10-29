using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RemoveParent : MonoBehaviour
{
    public float grenadeCreationTime;
    public float grenadeDespawnTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        grenadeCreationTime = Time.time;
        Debug.Log("Grenade created " + grenadeCreationTime);
    }
    public void Update()
    {
        if (Time.time - grenadeCreationTime >= grenadeDespawnTime)
        {
            Destroy(gameObject);
            Debug.Log("Grenade destroyed " + Time.time);
        }
    }

}
