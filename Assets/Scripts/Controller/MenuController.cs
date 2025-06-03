using UnityEngine;

public enum GameMode
{
    OneVsOne = 0,
    OneVsMany = 1,
    Custom = 2
}

public class MenuController : MonoBehaviour
{
    [SerializeField] private MainMenuView mainMenu;
    [SerializeField] private OptionsMenuView optionsMenu;

    private GameMode selectedMode = GameMode.OneVsOne;

    private void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        mainMenu.playButton.onClick.AddListener(OpenFightScene);
        mainMenu.optionsButton.onClick.AddListener(OpenOptions);
        mainMenu.quitButton.onClick.AddListener(QuitGame);

        optionsMenu.oneVsOneButton.onClick.AddListener(() => SelectMode(GameMode.OneVsOne));
        optionsMenu.oneVsManyButton.onClick.AddListener(() => SelectMode(GameMode.OneVsMany));
        optionsMenu.backButton.onClick.AddListener(BackToMainMenu);

        // Mặc định chọn 1vs1
        SelectMode(GameMode.OneVsOne);
    }

    private void SelectMode(GameMode mode)
    {
        selectedMode = mode;
        Debug.Log("Selected Mode: " + selectedMode);
    }

    private void OpenFightScene()
    {
        Debug.Log("Opening fight scene with mode: " + selectedMode);
        PlayerPrefs.SetInt("SelectedGameMode", (int)selectedMode);
        PlayerPrefs.Save();

        SceneLoader.LoadFightScene();
    }

    private void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        Debug.Log("Options menu opened");
    }

    private void BackToMainMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        Debug.Log("Back to main menu");
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        SceneLoader.QuitGame();
    }
}
