using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI keyScoreText;
    public TextMeshProUGUI damageScoreText;
    public TextMeshProUGUI killScoreText;
    public TextMeshProUGUI deathScoreText;
    public TextMeshProUGUI timeScoreText;
    public ScoreTracker scoreTracker;
    // Start is called before the first frame update
    void Start()
    {
        scoreTracker = ScoreTracker.instance;
        keyScoreText.text = "Keys Collected: " + scoreTracker.keyScore;
        damageScoreText.text = "Damage dealt: " + scoreTracker.damageScore;
        killScoreText.text = "Kills: " + scoreTracker.killScore;
        deathScoreText.text = "Deaths: " + scoreTracker.deathScore;
        timeScoreText.text = "time elapsed: " + scoreTracker.timeScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
