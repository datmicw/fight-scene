using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform[] playerSpawnPoints;
    public Transform[] enemySpawnPoints;

    private GameMode gameMode;
    private FollowCamera cameraFollow;

    private List<GameObject> players = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();
    private int currentLevel = 1; // level bắt đầu
    private void Start()
    {
        cameraFollow = Camera.main.GetComponent<FollowCamera>();
        gameMode = (GameMode)PlayerPrefs.GetInt("GameMode", 0);
        Debug.Log("GameMode Selected: " + gameMode);

        if (!ValidateSpawnPoints()) return;
        StartGame();
    }

    public void StartGame()
    {
        ClearExisting();
        players.Clear();
        enemies.Clear();

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

        if (players.Count > 0 && cameraFollow != null)
            cameraFollow.SetTarget(players[0].transform);
    }

    private GameObject SpawnPlayer(Transform spawnPoint, bool isControlled, float hp, float dmg, float speed)
    {
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        player.tag = "Player";

        CharacterControllerBase ctrl = player.GetComponent<CharacterControllerBase>();
        if (ctrl != null)
            ctrl.InitializeModel(hp, speed, dmg, 1f);

        if (!isControlled)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;

            player.AddComponent<SimpleAIController>();
        }

        return player;
    }

    private GameObject SpawnEnemy(Transform spawnPoint, Transform target, float hp, float dmg, float speed)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position + RandomOffset(), spawnPoint.rotation);
        enemy.tag = "Enemy";

        EnemyController ctrl = enemy.GetComponent<EnemyController>();
        if (ctrl != null)
        {
            ctrl.SetTarget(target);
            ctrl.InitializeModel(hp, speed, dmg, 1f);
        }

        return enemy;
    }

    private Vector3 RandomOffset()
    {
        return new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
    }

    private bool ValidateSpawnPoints()
    {
        return playerSpawnPoints.Length > 0 && enemySpawnPoints.Length > 0;
    }

    private void ClearExisting()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Player")) Destroy(go);
        foreach (var go in GameObject.FindGameObjectsWithTag("Enemy")) Destroy(go);
    }

    public void OnEnemyKilled(GameObject enemy)
    {
        Debug.ClearDeveloperConsole();
        if (enemies.Contains(enemy))
            enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            Debug.Log("Level " + currentLevel + " cleared!");
            Invoke(nameof(NextLevel), 2f);
        }
    }
    public void OnPlayerKilled(GameObject player)
    {
        Debug.ClearDeveloperConsole();
        if (players.Contains(player))
            players.Remove(player);

        if (players.Count == 0)
        {
            WinLoseUI.Instance.ShowResult(false);

        Time.timeScale = 0f;
        }

    }

    private void NextLevel()
    {
        currentLevel++;
        Debug.Log("Starting Level " + currentLevel);

        foreach (var player in players)
        {
            var character = player.GetComponent<CharacterControllerBase>();
            if (character != null) character.ResetHealth();
        }

        StartGame();
    }

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
