using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool canDamagePlayer = false;
    public int damage = 10;
    private void OnCollisionEnter(Collision collision)
    {
        if (canDamagePlayer == false)
            return;
        if(collision.transform.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.health -= damage;
            canDamagePlayer = false;
            if(player.health < 0)
            {
                // END GAME
            }
            
        }
    }

}
