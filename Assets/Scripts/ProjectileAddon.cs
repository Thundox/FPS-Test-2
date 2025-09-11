using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public int damage;
    private Rigidbody rb;
    private bool targetHit;
    public bool isSticky;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // make sure to only stick to the first target hit
        if (targetHit == true)
        {
            return;
        }
        else
            targetHit = true;

        // check if you hit an enemy
        if (collision.gameObject.GetComponent<BasicEnemy>() != null)
        {
            BasicEnemy enemy = collision.gameObject.GetComponent<BasicEnemy>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (isSticky == true)
        {
            ContactPoint contact = collision.contacts[0];
            transform.position = contact.point;

            // make sure projectile sticks to surface
            rb.isKinematic = true;

            // make sure projectile moves with target
            transform.SetParent(collision.transform);
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
