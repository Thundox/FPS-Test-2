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
            if (player.health <= 0)
            {
                player.playerDeath();
                Debug.Log("Grenade hit Player");
            }
        }
    }
}
