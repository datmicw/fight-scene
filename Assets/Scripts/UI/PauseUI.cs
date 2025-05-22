using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public GameObject pauseUI;

    private void Start()
    {
        pauseUI.SetActive(false);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (pauseUI.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
