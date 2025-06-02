using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    public Button playButton; // button bắt đầu game
    public Button optionsButton; // button vào menu options
    public Button quitButton; // button thoát game

    public void SetActive(bool active) => gameObject.SetActive(active);
}
