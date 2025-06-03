using UnityEngine;
using UnityEngine.SceneManagement;

// điều khiển menu chính trong game
public class MenuController : MonoBehaviour
{
    [SerializeField] private MainMenuView mainMenu; // tham chiếu đến giao diện menu chính
    [SerializeField] private OptionsMenuView optionsMenu; // tham chiếu đến giao diện menu tùy chọn
    private readonly MenuModel menuModel = new MenuModel(); // model quản lý trạng thái menu

    private void Start()
    {
        mainMenu.SetActive(true); // hiển thị menu chính khi bắt đầu
        optionsMenu.SetActive(false); // ẩn menu tùy chọn khi bắt đầu

        // gán sự kiện cho các nút trong menu chính
        mainMenu.playButton.onClick.AddListener(StartGame);
        mainMenu.optionsButton.onClick.AddListener(OpenOptions);
        mainMenu.quitButton.onClick.AddListener(QuitGame);

        // gán sự kiện cho các nút trong menu tùy chọn
        optionsMenu.backButton.onClick.AddListener(BackToMainMenu);
        optionsMenu.oneVsOneButton.onClick.AddListener(StartOneVsOneMode);
        optionsMenu.oneVsManyButton.onClick.AddListener(StartOneVsManyMode);
    }

    // bắt đầu game
    private void StartGame()
    {
        SceneLoader.LoadFightScene();
        Debug.Log("Game started");
    }

    // mở menu tùy chọn
    private void OpenOptions()
    {
        menuModel.SetMenu(MenuModel.MenuState.Options);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        Debug.Log("Options menu opened");
    }

    // quay lại menu chính
    private void BackToMainMenu()
    {
        menuModel.SetMenu(MenuModel.MenuState.Main);
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

    // bắt đầu chế độ một đấu một
    private void StartOneVsOneMode()
    {
        Debug.Log("Starting One vs One mode...");
    }

    // bắt đầu chế độ một đấu nhiều
    private void StartOneVsManyMode()
    {
        Debug.Log("Starting One vs Many mode...");
    }
}
