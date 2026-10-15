using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileLifetime;
    public float projectileTimer;
    public int projectileDamage;
    public float projectileDamageCooldown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        projectileTimer = 0;
        projectileDamageCooldown = 0;
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

        if (projectileDamageCooldown >= 0)
        {
            projectileDamageCooldown -= Time.deltaTime;
        }

        

        // Projectile hit detection

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);   
        }

        else if (other.CompareTag("Zombie"))
        {
            ScoreTracker myScoreTracker = FindObjectOfType<ScoreTracker>();
            Zombie hitZombie = other.transform.root.GetComponent<Zombie>();
            if (hitZombie.zombiePlasmaDamageCooldown == false)
            {
                hitZombie.TriggerRagdoll(Vector3.zero, Vector3.zero);
                if (hitZombie.zombieHealth > projectileDamage)
                {
                    myScoreTracker.damageScore += projectileDamage;
                }
                else if (hitZombie.zombieHealth > 0)
                {
                    myScoreTracker.damageScore += hitZombie.zombieHealth;
                    myScoreTracker.killScore += 1;

                }
                hitZombie.HitByPlasma(projectileDamage);
                hitZombie.zombiePlasmaDamageCooldown = true;
                Destroy(gameObject);
            }
            
        }
    }
}
