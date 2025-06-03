using UnityEngine;
using UnityEngine.SceneManagement;

// lớp tĩnh để quản lý việc chuyển cảnh
public static class SceneLoader
{
    // hàm để chuyển sang cảnh chiến đấu
    public static void LoadFightScene()
    {
        SceneManager.LoadScene("FightScene");
    }

    // hàm để thoát game
    public static void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
