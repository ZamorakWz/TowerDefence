using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static LevelDataSO;

public class WaveManager : MonoBehaviour
{
    public static event UnityAction OnWaveCompleted;
    public static event UnityAction OnAllWavesCompleted;

    public static event UnityAction<int, int> OnWaveCountChanged;
    public static event UnityAction<int> OnRemainingTimeChanged;

    [SerializeField] private LevelDataSO currentLevelData;

    [SerializeField] private EnemySpawnController enemySpawnController;
    [SerializeField] private int currentWaveIndex = 0;

    private void OnEnable()
    {
        EnemySpawnController.OnAllEnemiesDefeated += HandleWaveCompleted;
    }

    private void OnDisable()
    {
        EnemySpawnController.OnAllEnemiesDefeated -= HandleWaveCompleted;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartWave();
        }
    }

    public void StartWave()
    {
        if (currentWaveIndex < currentLevelData.waves.Count)
        {
            StartCoroutine(SpawnEnemiesForWave(currentLevelData.waves[currentWaveIndex]));
            currentWaveIndex++;
            OnWaveCountChanged?.Invoke(currentLevelData.waves.Count, currentWaveIndex);
        }
        else
        {
            OnAllWavesCompleted.Invoke();
            Debug.Log("All Waves are Defeated!");
        }
    }

    private void HandleWaveCompleted()
    {
        OnWaveCompleted?.Invoke();

        StartCoroutine(StartNextWaveWithDelay());
        Debug.Log("Current Wave Completed!");
    }

    private IEnumerator StartNextWaveWithDelay()
    {
        int remainingTime = currentLevelData.timeBetweenWaves;

        while (remainingTime >= 0)
        {
            OnRemainingTimeChanged?.Invoke(remainingTime);

            yield return new WaitForSeconds(1f);

            remainingTime--;
        }

        StartWave();
    }

    private IEnumerator SpawnEnemiesForWave(WaveConfig waveConfig)
    {
        foreach (var config in waveConfig.enemyConfigs)
        {
            //yield return StartCoroutine(enemySpawnController.SpawnEnemyRoutine(config));
            yield return StartCoroutine(enemySpawnController.CreateEnemyPool(config));
        }
    }
}