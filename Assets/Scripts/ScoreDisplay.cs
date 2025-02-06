using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI keyScoreText;
    public TextMeshProUGUI damageScoreText;
    public ScoreTracker scoreTracker;
    // Start is called before the first frame update
    void Start()
    {
        scoreTracker = ScoreTracker.instance;
        keyScoreText.text = "Key Score: " + scoreTracker.keyScore;
        damageScoreText.text = "Damage Score: " + scoreTracker.damageScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
