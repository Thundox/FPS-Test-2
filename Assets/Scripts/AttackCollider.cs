using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool canDamagePlayer = false;
    public int damage = 10;
    public Zombie myZombie;
    private void OnCollisionEnter(Collision collision)
    {

        if (canDamagePlayer == false)
            return;
        if (collision.transform.tag == "Wall")
        {
            Debug.Log("HIT WALL");
            myZombie.SetStateToHitWall();
            myZombie.DisableDamage();
        }
        if (collision.transform.tag == "Player")
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.health -= damage;
            canDamagePlayer = false;
            if (player.health <= 0)
            {
                player.playerDeath();
                Debug.Log("Hit Player");
                // END GAME
            }

        }

    }

}
