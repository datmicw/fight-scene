using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameMode CurrentMode { get; private set; }
    public int CurrentLevel { get; private set; } = 1;

    public int MaxLevel = 10;

    // tham chiếu đến prefab player và enemy
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    // danh sách enemy hiện có trên scene
    private List<GameObject> enemies = new List<GameObject>();

    private GameObject playerInstance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // tìm player và enemy có sẵn trong scene theo tag
        playerInstance = GameObject.FindWithTag("Player");

        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = new List<GameObject>(enemiesInScene);
    }

    // khởi tạo level theo mode
    public void StartLevel(GameMode mode, int level)
    {
        CurrentMode = mode;
        CurrentLevel = Mathf.Clamp(level, 1, MaxLevel);

        ClearLevel();

        SpawnPlayer();

        switch (CurrentMode)
        {
            case GameMode.OneVsOne:
                SpawnEnemies(1);
                break;
            case GameMode.OneVsMany:
                int enemyCount = Mathf.Min(50, 3 + CurrentLevel * 4); // tăng dần số enemy, tối đa 50
                Debug.Log($"Spawning {enemyCount} enemies for OneVsMany mode at level {CurrentLevel}");
                SpawnEnemies(enemyCount);
                break;
            default:
                Debug.LogError("mode chưa được hỗ trợ");
                break;
        }

        SetupLevelDifficulty();
    }

    // xóa dữ liệu level hiện tại
    private void ClearLevel()
    {
        // nếu player có sẵn trong scene, giữ lại, không destroy
        if (playerInstance != null)
        {
            playerInstance.transform.position = Vector3.zero; // đặt lại vị trí nếu cần
        }

        // xóa toàn bộ enemy hiện tại
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        enemies.Clear();
    }

    // spawn player nếu chưa có
    private void SpawnPlayer()
    {
        if (playerInstance == null)
        {
            playerInstance = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("spawn player mới");
        }
        else
        {
            Debug.Log("player đã có sẵn trong scene");
        }
    }

    // spawn enemy theo số lượng chỉ định
    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = GenerateEnemySpawnPosition(i, count);
            var enemy = Instantiate(EnemyPrefab, pos, Quaternion.identity);
            enemies.Add(enemy);
        }
        Debug.Log($"spawned {count} enemies");
    }

    // tạo vị trí spawn enemy theo index và tổng số lượng
    private Vector3 GenerateEnemySpawnPosition(int index, int totalCount)
    {
        float radius = 5 + CurrentLevel * 2;
        float angle = (360f / Mathf.Max(1, totalCount)) * index;
        float rad = Mathf.Deg2Rad * angle;
        Vector3 pos = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
        return pos;
    }

    // tạo vị trí spawn enemy khác nhau, tăng tính đa dạng
    private Vector3 GenerateEnemySpawnPosition(int index)
    {
        float radius = 5 + CurrentLevel * 2;
        float angle = (360f / Mathf.Max(1, enemies.Count + 1)) * index;
        float rad = Mathf.Deg2Rad * angle;
        Vector3 pos = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
        return pos;
    }

    // thiết lập độ khó cho level hiện tại
    private void SetupLevelDifficulty()
    {
        foreach (var enemyObj in enemies)
        {
            var enemyCtrl = enemyObj.GetComponent<BoxingEnemyAI>();
            if (enemyCtrl != null)
            {
                // tăng các thông số theo level
                float health = 50 + CurrentLevel * 20;
                float damage = 5 + CurrentLevel * 3;
                float speed = 2 + CurrentLevel * 0.5f;
                float cooldown = Mathf.Max(0.5f, 2f - CurrentLevel * 0.1f);

                enemyCtrl.InitializeModel(health, speed, damage, cooldown);
            }
        }
    }

    // chuyển sang level tiếp theo
    public void NextLevel()
    {
        if (CurrentLevel < MaxLevel)
        {
            StartLevel(CurrentMode, CurrentLevel + 1);
        }
        else
        {
            Debug.Log("đã đạt max level.");
        }
    }
}
