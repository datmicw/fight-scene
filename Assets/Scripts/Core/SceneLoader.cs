using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // Tên scene fight trong Build Settings, thay đổi nếu khác
    private const string FightSceneName = "FightScene";

    public static void LoadFightScene()
    {
        SceneManager.LoadScene(FightSceneName);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
