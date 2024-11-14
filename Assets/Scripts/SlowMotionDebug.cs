using UnityEngine;

public class SlowMotionDebug : MonoBehaviour
{
    public float slowMotionTimeScale = 0.33f;
    public float speedUpTimeScale = 3f;
    public bool isSlowMotion = false;
    public bool isSpeedUp = false;

    void Update()
    {
        // Toggle slow motion on/off with the 'T' key
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleSlowMotion();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ToggleSpeedUp();
        }
    }

    void ToggleSlowMotion()
    {
        isSlowMotion = !isSlowMotion; // flips true to false, false to true.
        Time.timeScale = isSlowMotion ? slowMotionTimeScale : 1f;
        
        if(isSlowMotion == true)
        {
            Time.timeScale = slowMotionTimeScale;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    void ToggleSpeedUp()
    {
        isSlowMotion = false;
        isSpeedUp = !isSpeedUp;
        Time.timeScale = isSpeedUp ? speedUpTimeScale : 1f;
    }
}