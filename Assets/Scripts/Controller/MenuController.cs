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

    [Header("Menu Views")]
    [SerializeField] private MainMenuView mainMenu;
    [SerializeField] private OptionsMenuView optionsMenu;

    private GameMode selectedMode = GameMode.OneVsOne;

    private void Start()
    {
        if (clickSound == null)
        {
            Debug.LogWarning("clickSound is null. Gán âm thanh trong Inspector.");
        }

        if (audioSource == null)
        {
            Debug.LogWarning("audioSource is null. Gán AudioSource trong Inspector.");
        }

        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        mainMenu.playButton.onClick.AddListener(() => { PlayClickSound(); OpenFightScene(); });
        mainMenu.optionsButton.onClick.AddListener(() => { PlayClickSound(); OpenOptions(); });
        mainMenu.quitButton.onClick.AddListener(() => { PlayClickSound(); QuitGame(); });

        optionsMenu.oneVsOneButton.onClick.AddListener(() => { PlayClickSound(); SelectMode(GameMode.OneVsOne); });
        optionsMenu.oneVsManyButton.onClick.AddListener(() => { PlayClickSound(); SelectMode(GameMode.OneVsMany); });
        optionsMenu.backButton.onClick.AddListener(() => { PlayClickSound(); BackToMainMenu(); });

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

    private void PlayClickSound()
    {
        if (audioSource != null && audioSource.enabled && audioSource.gameObject.activeInHierarchy)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("Cannot play sound: AudioSource is disabled or inactive.");
        }
    }
}
