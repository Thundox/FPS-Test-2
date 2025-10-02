using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    //Global variables / Attributes.
    public int damage;
    public HashSet<Zombie> hashsetZombiesHit = new HashSet<Zombie>();

    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Zombie")
        {
            Zombie zombieHit = other.transform.root.GetComponent<Zombie>();
            if(hashsetZombiesHit.Add(zombieHit))
            {
                zombieHit.zombieHealth -= damage;
                zombieHit.TriggerRagdoll(Vector3.zero, Vector3.back);
                //Disable collider
                transform.GetComponent<Collider>().enabled = false;
                //Destroy this exposion after 1 second
                
            }
        }
        //TODO Player damage.
    }

}
