using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool hasExploded = false;
    public bool disabled = false;

    public GameObject smallExplosionPrefab;
    public GameObject largeExplosionPrefab;
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
        else if(disabled == false)
        {
            ExplosionV2();
            //explode();
        }
    }

    public void ExplosionV2()
    {
        // Creates a copy of the prefab,  at this location,    with this rotation. (Quaternion.Identity = no rotation.
        Instantiate(smallExplosionPrefab, transform.position, Quaternion.identity);
        Instantiate(largeExplosionPrefab, transform.position, Quaternion.identity);

        disabled = true;
        Destroy(gameObject, 1f);
        
    }
    //public void explode()
    //{
    //    // If explosion collider is missing create one
    //    if (grenadeExplosioncollider == null)
    //    {
    //        grenadeExplosioncollider = gameObject.AddComponent<SphereCollider>();
    //        grenadeExplosioncollider.center = Vector3.zero;
    //        grenadeExplosioncollider.isTrigger = true;
    //    }
    //    grenadeExplosioncollider.enabled = true;
    //    grenadeExplosioncollider.radius = grenadeExplosionRadius;
        
    //    hasExploded = true;

    //    Destroy(this.gameObject, 0.5f);
    //}
    //public void grenadeDealDamage()
    //{
    //    foreach (var zombie  in hashsetZombiesHit)
    //    {
    //        zombie.zombieHealth -= zombie.temporaryGrenadeDamage;
    //        //Debug.Log("Distance is " + grenadeExplosionDistance + "Radius is " + grenadeExplosionRadius);
    //        //Debug.Log("Grenade hit Zombie: " + other.gameObject);
    //        zombie.TriggerRagdoll(grenadeKnockback, Vector3.back);
    //        if (zombie.zombieHealth <= 0)
    //        {
    //            Debug.Log("Grenade killed Zombie");
    //        }
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        PlayerMovement player = other.transform.parent.gameObject.GetComponent<PlayerMovement>();
    //        player.health -= grenadeDamage;
    //        Debug.Log("Grenade hit Player");
    //        if (player.health <= 0)
    //        {
    //            player.playerDeath();
    //            Debug.Log("Grenade killed Player");
    //        }
    //    }
    //    if (other.tag == "Zombie")
    //    {
    //        Zombie zombie = other.transform.root.gameObject.GetComponent<Zombie>();
    //        hashsetZombiesHit.Add(zombie);
    //        if (zombie != null )
    //        {
    //            // Initialize damage tracking for this zombie
    //            zombie.tempGrenadeDamage = 0f;
    //        }
            
    //        if (zombie != null)
    //        {
    //            // Calculate damage for this specific limb hit
    //            float limbDistance = Vector3.Distance(transform.position, other.transform.position);
                
    //            float calculatedDamage = grenadeDamage * (1 - (limbDistance / grenadeExplosionRadius));
                
    //            // Keep only the highest damage for this zombie
    //            if (calculatedDamage > zombie.tempGrenadeDamage)
    //            {
    //                Debug.DrawLine(transform.position, other.transform.position, Color.red, 10f);
    //                zombie.tempGrenadeDamage = calculatedDamage;
    //            }
    //        }
    //    }
    //}
    

}    //void LateUpdate()
    //{
    //    // Only apply damage once after explosion and if we have zombies to damage
    //    if (hasExploded && hashsetZombiesHit.Count > 0 && disabled == false)
    //    {
    //        // Apply the highest damage to each zombie
    //        foreach (Zombie zombie in hashsetZombiesHit)
    //        {
    //            zombie.zombieHealth -= (int)zombie.tempGrenadeDamage;
                
    //            Debug.Log($"Grenade dealt {(int)zombie.tempGrenadeDamage} damage to Zombie (max from all limbs)");
                
    //            zombie.TriggerRagdoll(grenadeKnockback, Vector3.back);
                
    //            if (zombie.zombieHealth <= 0)
    //            {
    //                Debug.Log("Grenade killed Zombie");
    //            }
                
    //            // Reset damage tracking
    //            zombie.tempGrenadeDamage = 0f;
    //        }
            
    //        // Clear hit set and mark as processed
    //        hashsetZombiesHit.Clear();
    //        hasExploded = false;
    //        disabled = true;
    //    }
    //}
