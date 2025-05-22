using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeSelectionUI : MonoBehaviour
{
    public Button btn1vs1;
    public Button btn1vsMany;
    public Button btnManyvsMany;

    private void Start()
    {
        btn1vs1.onClick.AddListener(() => SelectMode(GameMode.OneVsOne));
        btn1vsMany.onClick.AddListener(() => SelectMode(GameMode.OneVsMany));
        btnManyvsMany.onClick.AddListener(() => SelectMode(GameMode.ManyVsMany));
    }

    private void SelectMode(GameMode mode)
    {
        PlayerPrefs.SetInt("GameMode", (int)mode);
        SceneManager.LoadScene("FightScene"); 
    }
}
