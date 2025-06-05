using UnityEngine;
using UnityEngine.SceneManagement;

// lớp static để quản lý việc chuyển đổi scene
public static class SceneLoader
{
    // tên scene cho trận đấu
    private const string FightSceneName = "FightScene";

    // hàm để chuyển sang scene trận đấu
    public static void LoadFightScene()
    {
        SceneManager.LoadScene(FightSceneName);
    }

    // hàm để thoát game
    public static void QuitGame()
    {
        Application.Quit();
    }
}
