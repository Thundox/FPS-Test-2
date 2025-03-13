using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI keyScoreText;
    public TextMeshProUGUI damageScoreText;
    public TextMeshProUGUI killScoreText;
    public TextMeshProUGUI deathScoreText;
    public TextMeshProUGUI timeScoreText;
    public ScoreTracker scoreTracker;
    public float animationDuration;
    public Image damageScoreBar;
    public Image killScoreBar;

    // Start is called before the first frame update
    void Start()
    {
        scoreTracker = ScoreTracker.instance;
        keyScoreText.text = "Keys Collected: " + scoreTracker.keyScore;
        damageScoreText.text = "Damage dealt: " + scoreTracker.damageScore;
        killScoreText.text = "Kills: " + scoreTracker.killScore;
        deathScoreText.text = "Deaths: " + scoreTracker.deathScore;
        timeScoreText.text = "time elapsed: " + scoreTracker.timeScore;
        StartCoroutine(AnimateScore(scoreTracker.killScore,7,killScoreText, killScoreBar));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator AnimateScore()
    {
        float elapsedTime = 0f;
        int currentScore = 0;
        while (elapsedTime <= animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;

            currentScore = Mathf.FloorToInt(Mathf.Lerp(0, scoreTracker.damageScore, progress));
            damageScoreBar.fillAmount = Mathf.Lerp(0,  1, progress);
            damageScoreText.text = currentScore.ToString();
            yield return null;
        }
        // Makes sure it displays the correct score at end of animation
        damageScoreText.text = scoreTracker.damageScore.ToString();
    }

    public IEnumerator AnimateScore(int score,int maxBarScore,TextMeshProUGUI scoreText,Image scoreBar)
    {
        float barFinalFillAmount = (float)score / (float)maxBarScore;
        float elapsedTime = 0f;
        int currentScore = 0;
        while (elapsedTime <= animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;

            currentScore = Mathf.FloorToInt(Mathf.Lerp(0, score, progress));

            scoreBar.fillAmount = Mathf.Lerp(0, barFinalFillAmount, progress);
            scoreText.text = currentScore.ToString();
            yield return null;
        }
        // Makes sure it displays the correct score at end of animation
        scoreText.text = score.ToString();
    }
}
