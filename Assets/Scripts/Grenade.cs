using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float grenadeTimer;
    public GameObject grenadeGameObject;
    public SphereCollider grenadeExplosioncollider;
    public float grenadeExplosionRadius;
    public int grenadeDamage;
    public Vector3 grenadeKnockback;
    public HashSet<Zombie> hashsetZombiesHit = new HashSet<Zombie>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grenadeTimer > 0)
        {
            grenadeTimer -= Time.deltaTime;
        }
        else
        {
            explode();
        }
    }

    public void explode()
    {
        // If explosion collider is missing create one
        if (grenadeExplosioncollider == null)
        {
            grenadeExplosioncollider = gameObject.AddComponent<SphereCollider>();
            grenadeExplosioncollider.center = Vector3.zero;
            grenadeExplosioncollider.isTrigger = true;
        }
        grenadeExplosioncollider.enabled = true;
        grenadeExplosioncollider.radius = grenadeExplosionRadius;
        
        Destroy(this.gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.tag == "Player")
        {
            PlayerMovement player = other.transform.parent.gameObject.GetComponent<PlayerMovement>();
            player.health -= grenadeDamage;
            Debug.Log("Grenade hit Player");
            if (player.health <= 0)
            {
                player.playerDeath();
                Debug.Log("Grenade killed Player");
            }
        }
        if (other.tag == "Zombie")
        {
            Zombie zombie = other.transform.root.gameObject.GetComponent<Zombie>();
            if (zombie != null && hashsetZombiesHit.Add(zombie))
            {
                zombie.zombieHealth -= grenadeDamage;
                Debug.Log("Grenade hit Zombie: " + other.gameObject);
                zombie.TriggerRagdoll(grenadeKnockback, Vector3.back);
                if (zombie.zombieHealth <= 0)
                {
                    Debug.Log("Grenade killed Zombie");
                }
            }
            
            //Zombie zombieHitPoint = 
            //zombie.TriggerRagdoll(grenadeKnockback, Vector3.zero);

        }
    }
}
