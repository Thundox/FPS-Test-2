using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public int ammoAmount;
    public Shoot myShoot;
    // Start is called before the first frame update
    void Start()
    {
        myShoot = FindFirstObjectByType<Shoot>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            myShoot.playerSpareAmmo = myShoot.playerSpareAmmo + ammoAmount;
            Destroy(gameObject);
        }
    }

}
