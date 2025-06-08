using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuView : MonoBehaviour
{
    // [SerializeField] private Canvas optionsMenuCanvas;

    public Button oneVsOneButton;
    public Button oneVsManyButton;
    public Button backButton;

    public void SetActive(bool active) => gameObject.SetActive(active);
}
