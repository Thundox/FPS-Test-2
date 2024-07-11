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
}
