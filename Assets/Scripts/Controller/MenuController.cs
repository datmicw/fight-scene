using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MainMenuView mainMenu;
    [SerializeField] private OptionsMenuView optionsMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        mainMenu.playButton.onClick.AddListener(StartGame);
        mainMenu.optionsButton.onClick.AddListener(OpenOptions);
        mainMenu.quitButton.onClick.AddListener(QuitGame);

        optionsMenu.backButton.onClick.AddListener(BackToMainMenu);
        optionsMenu.oneVsOneButton.onClick.AddListener(StartOneVsOneMode);
        optionsMenu.oneVsManyButton.onClick.AddListener(StartOneVsManyMode);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("FightScene");
    }

    private void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    private void BackToMainMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void StartOneVsOneMode()
    {
        Debug.Log("Starting One vs One mode...");
    }

    private void StartOneVsManyMode()
    {
        Debug.Log("Starting One vs Many mode...");
    }
}
