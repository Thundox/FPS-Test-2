using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int lockNumber;
    public Animator myAnimator;
    public AudioSource audioDoorOpen;
    // Start is called before the first frame update
    void Start()
    {
        audioDoorOpen = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
           bool hasKey = other.GetComponent<KeysPlayer>().getKey(lockNumber);
            if (hasKey)
            {
                myAnimator.SetTrigger("openDoor");
                audioDoorOpen.Play();
            }
        }
        
    }
}
