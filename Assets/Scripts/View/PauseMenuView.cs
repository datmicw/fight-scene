using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;

    public void SetActive(bool active) => gameObject.SetActive(active);
}
