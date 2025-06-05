using UnityEngine;

// định nghĩa các chế độ chơi
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
        // bật menu chính, tắt menu tùy chọn khi bắt đầu
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        // gán sự kiện cho các nút trong menu chính
        mainMenu.playButton.onClick.AddListener(OpenFightScene);
        mainMenu.optionsButton.onClick.AddListener(OpenOptions);
        mainMenu.quitButton.onClick.AddListener(QuitGame);

        // gán sự kiện cho các nút trong menu tùy chọn
        optionsMenu.oneVsOneButton.onClick.AddListener(() => SelectMode(GameMode.OneVsOne));
        optionsMenu.oneVsManyButton.onClick.AddListener(() => SelectMode(GameMode.OneVsMany));
        optionsMenu.backButton.onClick.AddListener(BackToMainMenu);

        // mặc định chọn chế độ 1vs1
        SelectMode(GameMode.OneVsOne);
    }

    // chọn chế độ chơi
    private void SelectMode(GameMode mode)
    {
        selectedMode = mode;
        Debug.Log("Selected Mode: " + selectedMode);
    }

    // mở màn chơi
    private void OpenFightScene()
    {
        Debug.Log("Opening fight scene with mode: " + selectedMode);
        PlayerPrefs.SetInt("SelectedGameMode", (int)selectedMode);
        PlayerPrefs.Save();
        SceneLoader.LoadFightScene();
    }

    // mở menu tùy chọn
    private void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        Debug.Log("Options menu opened");
    }

    // quay lại menu chính
    private void BackToMainMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        Debug.Log("Back to main menu");
    }

    // thoát game
    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        SceneLoader.QuitGame();
    }
}
