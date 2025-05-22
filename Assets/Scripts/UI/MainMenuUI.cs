using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject modeSelectionUI;
    public GameObject mainMenuUI;

    public void OnStartButton() {
        mainMenuUI.SetActive(false);
        modeSelectionUI.SetActive(true);
    }
    public void OnExitButton()
    {
        Application.Quit();
    }
}
