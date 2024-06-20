using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysPlayer : MonoBehaviour
{
    public AudioSource keyPickedUp;
    public bool Key1;
    public bool Key2;
    public bool Key3;
    public bool Key4;
    public bool Key5;
    // Add switch function to assigning keys
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setKey(int keyToSet)
    {
        switch (keyToSet)
        {
            case 1:
                Key1 = true;
                break;
            case 2:
                Key2 = true;
                break;
            case 3:
                Key3 = true;
                break;
            case 4:
                Key4 = true;
                break;
            case 5:
                Key5 = true;
                break;
        }
    }
    public bool getKey(int keyToGet)
    {
        switch (keyToGet)
        {
            case 1:
                return Key1;
                
            case 2:
                return Key2;
                
            case 3:
                return Key3;
                
            case 4:
                return Key4;
                
            case 5:
                return Key5;
                
        }
        return false;
    }
}
