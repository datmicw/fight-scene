using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameMode currentMode = GameMode.OneVsOne;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform playerSpawnPoint;
    public List<Transform> enemySpawnPoints;
    private GameObject player;
    private int currentLevel = 1;
    private int aliveEnemies = 0;

    private void Awake()
    {
        int savedMode = PlayerPrefs.GetInt("SelectedGameMode", 0);
        currentMode = (GameMode)savedMode;
    }
    private IEnumerator Start()
    {
        SpawnPlayer();

        while (PlayerManager.Instance == null || PlayerManager.Instance.Player == null)
        {
            Debug.Log("Đang chờ PlayerManager và Player...");
            yield return null;
        }

        SpawnEnemies();
    }
    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.RegisterPlayer(player);
        }
    }
    private void SpawnEnemies()
    {
        if (PlayerManager.Instance == null || PlayerManager.Instance.Player == null)
        {
            Debug.LogError("PlayerManager hoặc Player chưa sẵn sàng!"); return;
        }
        Transform playerTransform = PlayerManager.Instance.Player.transform;

        if (currentMode == GameMode.OneVsOne)
        {
            GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[0].position, Quaternion.identity);
            BoxingEnemyAI ai = enemy.GetComponent<BoxingEnemyAI>();
            if (ai != null)
            {
                ai.SetTarget(playerTransform);
                ai.onDeath += OnEnemyDeath;
            }
            aliveEnemies = 1;
        }
        else if (currentMode == GameMode.OneVsMany)
        {
            int enemyCount = Mathf.Min(currentLevel + 1, enemySpawnPoints.Count);
            aliveEnemies = enemyCount;

            for (int i = 0; i < enemyCount; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);
                BoxingEnemyAI ai = enemy.GetComponent<BoxingEnemyAI>();
                if (ai != null)
                {
                    ai.SetTarget(playerTransform);
                    ai.onDeath += OnEnemyDeath;
                }
            }
        }
    }

    private void OnEnemyDeath()
    {
        aliveEnemies--;
        Debug.Log($"Enemy chết, còn lại: {aliveEnemies}");
        if (aliveEnemies <= 0)
        {
            Debug.Log("Level " + currentLevel + " cleared!");
            currentLevel++;
            Debug.Log("Moving to level " + currentLevel);

            Invoke(nameof(SpawnEnemies), 5f); // Đợi 5 giây rồi spawn level mới
        }
    }
}
