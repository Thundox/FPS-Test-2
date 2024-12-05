using UnityEngine;
using UnityEngine.UI;

public class FullScreenImage : MonoBehaviour
{
    private void Awake()
    {
        // Ensure the image fills the screen
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;  // Sets left and bottom
        rectTransform.offsetMax = Vector2.zero;  // Sets right and top
    }
}