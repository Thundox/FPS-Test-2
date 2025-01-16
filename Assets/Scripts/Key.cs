using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // update key if Player touches key
            KeysPlayer myKeyplayer = other.GetComponent<KeysPlayer>();
            myKeyplayer.setKey(keyNumber);
           myKeyplayer.keyPickedUp.Play();
            FindObjectOfType<ScoreTracker>().keyScore +=1;
            this.gameObject.SetActive(false); 
        }


    }
}
