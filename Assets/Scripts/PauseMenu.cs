using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI; // Reference to the Panel
    public static bool GameIsPaused = false;

    private PlayerCam _playerCam;
    private Shoot _shoot;

    void Start()
    {
        _playerCam = FindObjectOfType<PlayerCam>();
        _shoot = FindObjectOfType<Shoot>();
    }

    void Update()
    {
        // Check for the Escape key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the menu
        Time.timeScale = 1f;          // Resume game time
        GameIsPaused = false;

        // Re-enable player camera and shooting
        if (_playerCam != null) _playerCam.AllowPlayerCamMovement = true;
        if (_shoot != null) _shoot.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Show the menu
        Time.timeScale = 0f;          // Freeze game time
        GameIsPaused = true;

        // Disable player camera and shooting
        if (_playerCam != null) _playerCam.AllowPlayerCamMovement = false;
        if (_shoot != null) _shoot.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; // Ensure time is running before loading new scene
        SceneManager.LoadScene("Menu"); // Replace "Menu" with your main menu scene name
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
