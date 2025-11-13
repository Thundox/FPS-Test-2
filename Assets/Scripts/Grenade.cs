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
        Debug.Log("Grenade rotation is: " + transform.rotation.eulerAngles);
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
    

}
