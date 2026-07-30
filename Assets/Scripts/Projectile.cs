using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileLifetime;
    public float projectileTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        projectileTimer = 0;  
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
        projectileTimer += Time.deltaTime;
        if (projectileTimer >= projectileLifetime) 
        {
            Destroy(gameObject);
        }

    }
}
