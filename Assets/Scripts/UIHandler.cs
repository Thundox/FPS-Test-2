using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public Image deathImage;
    public float fadeDuration;
    public TextMeshProUGUI playerAmmo;
    public PlayerMovement myPlayerMovement;
    public Shoot myShoot;
    public Image healthBarImage;
    

    void Start()
    {
        playerAmmo.text = myShoot.playerAmmoInGun + "-" + myShoot.playerSpareAmmo;
    }

    private void Awake()
    {
        
        
    }

    void Update()
    {
        playerAmmo.text = myShoot.playerAmmoInGun + "-" + myShoot.playerSpareAmmo;
        healthBarImage.fillAmount = (float)myPlayerMovement.health / myPlayerMovement.maxHealth;
    }

    IEnumerator fadeImage()
    {
        float timePassed = 0f;
        Color currentColor = deathImage.color;
        Color targetColor = new Color(0f, 0f, 0f, 1f);

        while (timePassed < fadeDuration)
        {
            deathImage.color = Color.Lerp(currentColor, targetColor, timePassed / fadeDuration);
            timePassed += Time.deltaTime;
            yield return null; // Wait for next frame
        }
        deathImage.color = targetColor;
        SceneManager.LoadScene("SampleScene");
    }

    public void startFade()
    {
        StartCoroutine(fadeImage());
    }
}
