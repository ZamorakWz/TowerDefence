using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create New Level", menuName = "Level Data")]
public class LevelDataSO : ScriptableObject
{
    [Header("Level Settings")]
    public GameObject LevelPrefab;
    public int levelBeginnigGold;
    public int levelReward;
    public int levelBaseHealth;
    public int timeBetweenWaves;

    [System.Serializable]
    public class WaveConfig
    {
        [Header("How Many and What Type Enemies for This Wave")]
        public List<EnemySpawnController.EnemySpawnConfig> enemyConfigs;
    }

    [Header("Wave Configurations")]
    public List<WaveConfig> waves;
}