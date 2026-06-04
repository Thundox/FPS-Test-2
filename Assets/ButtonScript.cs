using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public string levelToLoad = string.Empty;
    
    public void loadScene()
    {
        SceneManager.LoadScene (levelToLoad);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game..."); // Logs to console to confirm it works in Editor
        Application.Quit();

        // This line allows you to stop play mode in the Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
