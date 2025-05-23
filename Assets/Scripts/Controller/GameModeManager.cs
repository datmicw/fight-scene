using UnityEngine;
using System.Collections.Generic;

// quản lý chế độ chơi và sinh ra người chơi, kẻ địch theo mode
public class GameModeManager : MonoBehaviour
{
    // prefab người chơi
    public GameObject playerPrefab;
    // prefab kẻ địch
    public GameObject enemyPrefab;
    // các điểm sinh ra người chơi
    public Transform[] playerSpawnPoints;
    // các điểm sinh ra kẻ địch
    public Transform[] enemySpawnPoints;

    // chế độ chơi hiện tại
    private GameMode gameMode;
    // script camera theo dõi người chơi
    private FollowCamera cameraFollow;

    // danh sách người chơi hiện tại
    private List<GameObject> players = new List<GameObject>();
    // danh sách kẻ địch hiện tại
    private List<GameObject> enemies = new List<GameObject>();
    // level hiện tại
    private int currentLevel = 1;

    private void Start()
    {
        cameraFollow = Camera.main.GetComponent<FollowCamera>();
        gameMode = (GameMode)PlayerPrefs.GetInt("GameMode", 0);
        Debug.Log("GameMode Selected: " + gameMode);

        // kiểm tra điểm spawn hợp lệ
        if (!ValidateSpawnPoints()) return;
        StartGame();
    }

    // bắt đầu game, sinh ra người chơi và kẻ địch theo mode
    public void StartGame()
    {
        ClearExisting();
        players.Clear();
        enemies.Clear();

        // lấy chỉ số cho level hiện tại
        GetStatsForLevel(
            currentLevel,
            out float enemyHp,
            out float enemyDmg,
            out float enemySpeed,
            out float playerHp,
            out float playerDmg,
            out float playerSpeed
        );

        switch (gameMode)
        {
            case GameMode.OneVsOne:
                players.Add(SpawnPlayer(playerSpawnPoints[0], true, playerHp, playerDmg, playerSpeed));
                enemies.Add(SpawnEnemy(enemySpawnPoints[0], players[0].transform, enemyHp, enemyDmg, enemySpeed));
                break;

            case GameMode.OneVsMany:
                GameObject player = SpawnPlayer(playerSpawnPoints[0], true, playerHp, playerDmg, playerSpeed);
                players.Add(player);
                foreach (Transform spawn in enemySpawnPoints)
                    enemies.Add(SpawnEnemy(spawn, player.transform, enemyHp, enemyDmg, enemySpeed));
                break;

            case GameMode.ManyVsMany:
                int count = Mathf.Min(playerSpawnPoints.Length, enemySpawnPoints.Length);
                for (int i = 0; i < count; i++)
                {
                    GameObject p = SpawnPlayer(playerSpawnPoints[i], i == 0, playerHp, playerDmg, playerSpeed);
                    GameObject e = SpawnEnemy(enemySpawnPoints[i], p.transform, enemyHp, enemyDmg, enemySpeed);
                    players.Add(p);
                    enemies.Add(e);
                }
                break;
        }

        // set camera theo dõi người chơi đầu tiên
        if (players.Count > 0 && cameraFollow != null)
            cameraFollow.SetTarget(players[0].transform);
    }

    // sinh ra người chơi tại vị trí spawnPoint
    // isControlled: true nếu là người chơi điều khiển, false thì dùng AI
    private GameObject SpawnPlayer(Transform spawnPoint, bool isControlled, float hp, float dmg, float speed)
    {
        if (ObjectPool.Instance == null) return null;

        GameObject player = ObjectPool.Instance.SpawnFromPool("Player", spawnPoint.position, spawnPoint.rotation);
        if (player == null) return null;

        player.tag = "Player";

        CharacterControllerBase ctrl = player.GetComponent<CharacterControllerBase>();
        if (ctrl != null)
            ctrl.InitializeModel(hp, speed, dmg, 1f);

        if (!isControlled)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;

            // thêm AI nếu không phải người chơi điều khiển
            if (!player.GetComponent<SimpleAIController>())
                player.AddComponent<SimpleAIController>();
        }

        return player;
    }

    // sinh ra kẻ địch tại vị trí spawnPoint, target là người chơi
    private GameObject SpawnEnemy(Transform spawnPoint, Transform target, float hp, float dmg, float speed)
    {
        if (ObjectPool.Instance == null) return null;

        GameObject enemy = ObjectPool.Instance.SpawnFromPool("Enemy", spawnPoint.position + RandomOffset(), spawnPoint.rotation);
        if (enemy == null) return null;

        enemy.tag = "Enemy";

        EnemyController ctrl = enemy.GetComponent<EnemyController>();
        if (ctrl != null)
        {
            ctrl.SetTarget(target);
            ctrl.InitializeModel(hp, speed, dmg, 1f);
        }

        return enemy;
    }

    // tạo vị trí random nhỏ để tránh trùng vị trí spawn
    private Vector3 RandomOffset()
    {
        return new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
    }

    // kiểm tra có đủ điểm spawn không
    private bool ValidateSpawnPoints()
    {
        return playerSpawnPoints.Length > 0 && enemySpawnPoints.Length > 0;
    }

    // tắt các object người chơi và kẻ địch cũ
    private void ClearExisting()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Player")) go.SetActive(false);
        foreach (var go in GameObject.FindGameObjectsWithTag("Enemy")) go.SetActive(false);
    }

    // gọi khi kẻ địch bị die
    public void OnEnemyKilled(GameObject enemy)
    {
        Debug.ClearDeveloperConsole();
        if (enemies.Contains(enemy))
            enemies.Remove(enemy);

        // nếu hết kẻ địch thì qua level mới
        if (enemies.Count == 0)
        {
            Debug.Log("Level " + currentLevel + " cleared!");
            Invoke(nameof(NextLevel), 2f);
        }
    }

    // gọi khi người chơi bị die
    public void OnPlayerKilled(GameObject player)
    {
        Debug.ClearDeveloperConsole();
        if (players.Contains(player))
            players.Remove(player);

        // nếu hết người chơi thì thua
        if (players.Count == 0)
        {
            WinLoseUI.Instance.ShowResult(false);
            Time.timeScale = 0f;
        }
    }

    // chuyển sang level tiếp theo
    private void NextLevel()
    {
        currentLevel++;
        Debug.Log("Starting Level " + currentLevel);

        // hồi máu cho người chơi còn sống
        foreach (var player in players)
        {
            var character = player.GetComponent<CharacterControllerBase>();
            if (character != null) character.ResetHealth();
        }

        StartGame();
    }

    // lấy chỉ số cho level
    private void GetStatsForLevel(
        int level,
        out float enemyHp,
        out float enemyDmg,
        out float enemySpeed,
        out float playerHp,
        out float playerDmg,
        out float playerSpeed)
    {
        enemyHp = 100 + level * 5f;
        enemyDmg = 5 + level * 0.8f;
        enemySpeed = 2f + level * 0.1f;

        playerHp = 100 + level * 6f;
        playerDmg = 10 + level * 0.8f;
        playerSpeed = 2.5f + level * 0.05f;
    }
}
