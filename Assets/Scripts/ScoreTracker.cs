using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public static ScoreTracker instance;
    public int playerScore = 0;
    public int keyScore = 0;
    public int damageScore = 0;
    public int killScore = 0;
    public int deathScore = 0;
    public int timeScore = 0;
    public int zombieCount = 0;
    public int totalZombieHealth = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Zombie[] zombieArray = FindObjectsOfType<Zombie>();
        zombieCount = zombieArray.Length;
        // [automatic zombie detection] make a Loop that goes through all zombies?
        instance.ResetScore();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ResetScore ()
    {
        playerScore = 0;
        keyScore = 0;
        damageScore = 0;
        killScore = 0;
        timeScore = 0;
    }
        
}
