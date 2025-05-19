using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData // quản lý level của game ba gồm 10 level
{
    public int levelNumber;
    public int playerCount;
    public int enemyCount;
    public float enemyHealthMultiplier;
    public float enemySpeedMultiplier;
}
