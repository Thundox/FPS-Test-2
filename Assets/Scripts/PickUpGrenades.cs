using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickUpGrenades : MonoBehaviour
{
    public int grenadeAmmoAmount;
    public ThrowingTutorial mythrowingTutorial;
    // Start is called before the first frame update
    void Start()
    {
        mythrowingTutorial = FindFirstObjectByType<ThrowingTutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mythrowingTutorial.totalThrows += grenadeAmmoAmount;
            Destroy(gameObject);
        }
    }
}
