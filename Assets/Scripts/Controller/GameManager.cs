using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelLoadView levelLoadView;
    public GameMode currentMode = GameMode.OneVsOne;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform playerSpawnPoint;
    public List<Transform> enemySpawnPoints;

    private GameObject player;
    private int currentLevel = 1;
    private int aliveEnemies = 0;

    // chỉ số cơ bản
    private float basePlayerHP = 100;
    private float basePlayerDmg = 10;
    private float baseEnemyHP = 100;
    private float baseEnemyDmg = 5;

    private void Awake()
    {
        // lấy chế độ chơi đã lưu từ PlayerPrefs
        int savedMode = PlayerPrefs.GetInt("SelectedGameMode", 0);
        currentMode = (GameMode)savedMode;
    }

    private IEnumerator Start()
    {
        // sinh ra player
        SpawnPlayer();

        // chờ PlayerManager và Player sẵn sàng
        while (PlayerManager.Instance == null || PlayerManager.Instance.Player == null)
        {
            Debug.Log("Đang chờ PlayerManager và Player...");
            yield return null;
        }

        // hiển thị thông tin level lên UI
        ShowLevelInfo();
    }

    private void ShowLevelInfo()
    {
        if (levelLoadView == null) return;

        // tính chỉ số cho level hiện tại
        ScaleStatsForLevel(out float playerHP, out float playerDmg, out float enemyHP, out float enemyDmg);

        // cập nhật text UI
        levelLoadView.levelText.text = $"LEVEL {currentLevel}";
        levelLoadView.playerText.text = $"PLAYER: HP: {Mathf.RoundToInt(playerHP)} | DMG: {Mathf.RoundToInt(playerDmg)}";
        levelLoadView.enemyText.text = $"ENEMY:  HP: {Mathf.RoundToInt(enemyHP)} | DMG: {Mathf.RoundToInt(enemyDmg)}";

        // bật UI level
        levelLoadView.SetActive(true);

        // gán sự kiện cho nút tiếp tục
        levelLoadView.continueButton.onClick.RemoveAllListeners();
        levelLoadView.continueButton.onClick.AddListener(() =>
        {
            // tắt UI và bắt đầu sinh enemy
            levelLoadView.SetActive(false);
            SpawnEnemies();
        });
    }

    private void ScaleStatsForLevel(out float playerHP, out float playerDmg, out float enemyHP, out float enemyDmg)
    {
        // player tăng chỉ số nhẹ mỗi level
        playerHP = basePlayerHP + currentLevel * 10f;
        playerDmg = basePlayerDmg + currentLevel * 2f;

        // enemy tăng mạnh hơn để giữ tỉ lệ 6:4
        enemyHP = baseEnemyHP + currentLevel * 20f;
        enemyDmg = baseEnemyDmg + currentLevel * 3.5f;
    }

    private void SpawnPlayer()
    {
        // sinh ra player tại vị trí spawn
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        // đăng ký player vào PlayerManager nếu có
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.RegisterPlayer(player);

        // khởi tạo chỉ số cho player
        var ctrl = player.GetComponent<CharacterControllerBase>();
        ScaleStatsForLevel(out float hp, out float dmg, out _, out _);
        ctrl.InitializeModel(hp, 5f, dmg, 1f);
    }

    private void SpawnEnemies()
    {
        // kiểm tra PlayerManager và Player đã sẵn sàng chưa
        if (PlayerManager.Instance == null || PlayerManager.Instance.Player == null)
        {
            Debug.LogError("PlayerManager hoặc Player chưa sẵn sàng!");
            return;
        }

        // lấy transform của player để enemy nhắm mục tiêu
        Transform playerTransform = PlayerManager.Instance.Player.transform;
        ScaleStatsForLevel(out _, out _, out float enemyHP, out float enemyDmg);

        if (currentMode == GameMode.OneVsOne)
        {
            // chế độ 1vs1: chỉ sinh 1 enemy
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[0].position, Quaternion.identity);
            SetupEnemy(enemy, playerTransform, enemyHP, enemyDmg);
            aliveEnemies = 1;
        }
        else if (currentMode == GameMode.OneVsMany)
        {
            // chế độ 1vsMany: sinh nhiều enemy tùy theo level
            int enemyCount = Mathf.Min(currentLevel + 1, enemySpawnPoints.Count);
            aliveEnemies = enemyCount;

            for (int i = 0; i < enemyCount; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);
                SetupEnemy(enemy, playerTransform, enemyHP, enemyDmg);
            }
        }
    }

    private void SetupEnemy(GameObject enemy, Transform target, float hp, float dmg)
    {
        // gán target cho AI của enemy
        BoxingEnemyAI ai = enemy.GetComponent<BoxingEnemyAI>();
        ai?.SetTarget(target);

        // khởi tạo chỉ số cho enemy
        var ctrl = enemy.GetComponent<CharacterControllerBase>();
        ctrl?.InitializeModel(hp, 4f, dmg, 1f);

        // đăng ký sự kiện chết cho enemy
        if (ai != null)
            ai.onDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath()
    {
        // giảm số lượng enemy còn sống
        aliveEnemies--;
        Debug.Log($"Enemy chết, còn lại: {aliveEnemies}");

        // nếu hết enemy thì qua level mới
        if (aliveEnemies <= 0)
        {
            Debug.Log($"Level {currentLevel} cleared!");
            currentLevel++;

            // hồi máu cho player
            var playerStats = PlayerManager.Instance.Player.GetComponent<CharacterControllerBase>();
            playerStats.ResetHealth();

            Debug.Log($"Player health reset to: {playerStats.GetHealth()}");
            ShowLevelInfo();
        }
    }
}
