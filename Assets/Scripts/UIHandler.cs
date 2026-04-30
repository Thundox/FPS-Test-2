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
    public float barMaskWidth;
    public RectTransform barMaskRectTransform;
    public RawImage healthBarImageRaw;


    void Start()
    {
        playerAmmo.text = myShoot.playerAmmoInGun + "-" + myShoot.playerSpareAmmo;
    }

    private void Awake()
    {
        barMaskWidth = barMaskRectTransform.sizeDelta.x;
        
    }

    void Update()
    {
        playerAmmo.text = myShoot.playerAmmoInGun + "-" + myShoot.playerSpareAmmo;
        // [Placeholder]
        healthBarImage.fillAmount = (float)myPlayerMovement.health / myPlayerMovement.maxHealth;
        // Health bar
        Rect uvRect = healthBarImageRaw.uvRect;
        uvRect.x -= 1f * Time.deltaTime;
        healthBarImageRaw.uvRect = uvRect; 
        
        Vector2 barMaskSizeDelta = barMaskRectTransform.sizeDelta;
        barMaskSizeDelta.x = (float)myPlayerMovement.health / myPlayerMovement.maxHealth * barMaskWidth;
        barMaskRectTransform.sizeDelta = barMaskSizeDelta;

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
