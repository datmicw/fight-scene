using UnityEngine;

public class GameModeLoader : MonoBehaviour
{
    public static GameMode SelectedMode;

    void Awake()
    {
        SelectedMode = (GameMode)PlayerPrefs.GetInt("GameMode", 0);
    }
}
