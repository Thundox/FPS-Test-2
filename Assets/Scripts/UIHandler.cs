using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour
{
    public Image fadeImage; // Assign this in the inspector with your UI Image
    public float fadeDuration = 2f; // Duration of the fade

    // Start is called before the first frame update
    void Start()
    {
        // Optionally start the fade effect immediately
        StartCoroutine(FadeToRed());
    }

    // Call this function to start the fade effect
    public void StartFadeToRed()
    {
        StartCoroutine(FadeToRed());
    }

    IEnumerator FadeToRed()
    {
        float elapsedTime = 0f;
        Color currentColor = fadeImage.color;
        Color targetColor = new Color(1f, 0f, 0f, 1f); // Target color is red with full alpha

        while (elapsedTime < fadeDuration)
        {
            // Lerp the color from current to target based on elapsed time
            fadeImage.color = Color.Lerp(currentColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        fadeImage.color = targetColor; // Ensure the final color is set
    }
}