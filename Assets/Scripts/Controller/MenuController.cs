using UnityEngine;

public enum GameMode
{
    OneVsOne = 0,
    OneVsMany = 1,
    Custom = 2
}

public class MenuController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Menu Views")]
    [SerializeField] private MainMenuView mainMenu;
    [SerializeField] private OptionsMenuView optionsMenu;
    [Header("Canvas References")]
    [SerializeField] private Canvas optionsMenuCanvas;
    [SerializeField] private Canvas mainMenuCanvas;
    [Header("Game Mode")]
    [SerializeField] private GameMode selectedMode = GameMode.OneVsOne;

    private void Start()
    {
        InitAudio();
        InitMenus();
        RegisterListeners();
        SelectMode(GameMode.OneVsOne);
    }

    private void InitAudio()
    {
        if (clickSound == null)
        {
            Debug.LogWarning("Click sound chưa được gán trong Inspector.");
        }

        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource chưa được gán trong Inspector.");
            return;
        }

        if (backgroundMusic == null)
        {
            backgroundMusic = Resources.Load<AudioClip>("Audio/BackgroundMusic");

            if (backgroundMusic == null)
            {
                Debug.LogError("Không tìm thấy BackgroundMusic trong Resources/Audio.");
                return;
            }
        }

        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void InitMenus()
    {
        if (mainMenu != null) mainMenu.SetActive(true);
        if (optionsMenu != null) optionsMenu.SetActive(false);
    }

    private void RegisterListeners()
    {
        if (mainMenu != null)
        {
            mainMenu.playButton.onClick.AddListener(() => { PlayClickSound(); OpenFightScene(); });
            mainMenu.optionsButton.onClick.AddListener(() => { PlayClickSound(); OpenOptions(); });
            mainMenu.quitButton.onClick.AddListener(() => { PlayClickSound(); QuitGame(); });
        }

        if (optionsMenu != null)
        {
            optionsMenu.oneVsOneButton.onClick.AddListener(() => { PlayClickSound(); SelectMode(GameMode.OneVsOne); });
            optionsMenu.oneVsManyButton.onClick.AddListener(() => { PlayClickSound(); SelectMode(GameMode.OneVsMany); });
            optionsMenu.backButton.onClick.AddListener(() => { PlayClickSound(); BackToMainMenu(); });
        }
    }

    private void PlayClickSound()
    {
        if (audioSource == null || clickSound == null) return;

        // Giảm khả năng chồng tiếng
        if (audioSource.isActiveAndEnabled && gameObject.activeInHierarchy)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private void SelectMode(GameMode mode)
    {
        selectedMode = mode;
        Debug.Log("Chế độ đã chọn: " + selectedMode);
    }

    private void OpenFightScene()
    {
        Debug.Log("Đang mở Fight Scene với mode: " + selectedMode);
        PlayerPrefs.SetInt("SelectedGameMode", (int)selectedMode);
        PlayerPrefs.Save();
        SceneLoader.LoadFightScene();
    }

    private void OpenOptions()
    {
        optionsMenuCanvas.enabled = true;
        optionsMenuCanvas.gameObject.SetActive(true);
        mainMenuCanvas.enabled = false;
        mainMenuCanvas.gameObject.SetActive(false);
        Debug.Log("Đã mở Options menu.");
    }

    private void BackToMainMenu()
    {
        optionsMenuCanvas.enabled = false;
        optionsMenuCanvas.gameObject.SetActive(false);
        mainMenuCanvas.enabled = true;
        mainMenuCanvas.gameObject.SetActive(true);
        Debug.Log("Quay lại Main Menu.");
    }

    private void QuitGame()
    {
        Debug.Log("Thoát game...");
        SceneLoader.QuitGame();
    }
}
