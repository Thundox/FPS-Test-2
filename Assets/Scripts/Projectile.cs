using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileLifetime;
    public float projectileTimer;
    public int projectileDamage;
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
        // projectile Speed
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
        // projectile Lifetime and timer
        projectileTimer += Time.deltaTime;
        if (projectileTimer >= projectileLifetime) 
        {
            Destroy(gameObject);
        }

        

        // Projectile hit detection

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);   
        }
    }
}
