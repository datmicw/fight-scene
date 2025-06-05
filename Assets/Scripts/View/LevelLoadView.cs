using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoadView : MonoBehaviour
{
    [SerializeField] private Canvas levelLoadCanvas;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI enemyText;
    public Button continueButton;

    public void SetActive(bool active) => levelLoadCanvas.gameObject.SetActive(active);
}
