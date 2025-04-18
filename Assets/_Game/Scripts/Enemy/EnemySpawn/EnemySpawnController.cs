using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnController : MonoBehaviour
{
    public static event Action OnAllEnemiesDefeated;
    public static event Action<int> OnEnemyCountChanged;

    [Header("Spawn Position of Enemy")]
    [SerializeField] private Vector3 _spawnPosition;

    [SerializeField] private EnemyObjectPool _enemyObjectPool;
    [SerializeField] private int _aliveEnemyCount;
    public int AliveEnemyCount
    {
        get { return _aliveEnemyCount; }
        set
        { 
            _aliveEnemyCount = Mathf.Max(0, value);

            OnEnemyCountChanged?.Invoke(_aliveEnemyCount);

            if (_aliveEnemyCount <= 0)
            {
                OnAllEnemiesDefeated?.Invoke();
            }
        }
    }

    public IEnumerator SpawnEnemyRoutine(EnemySpawnConfig config)
    {
        int counter = 0;
        while (counter < config.spawnCount)
        {
            GameObject obj = _enemyObjectPool.GetPooledEnemy(config.enemyType);

            if (obj != null)
            {
                obj.transform.position = _spawnPosition;
                counter++;
                AliveEnemyCount++;
            }

            yield return new WaitForSeconds(config.spawnInterval);
        }
    }

    //
    public IEnumerator CreateEnemyPool(EnemySpawnConfig config)
    {
        int currentDifficultyLevel = 0;

        while (currentDifficultyLevel < config.totalDifficultyLevel)
        {
            Array EnemyTypeLength = Enum.GetNames(typeof(EnemyObjectPool.EnemyType));

            int randomEnemyTypeIndex = UnityEngine.Random.Range(0, EnemyTypeLength.Length);

            GameObject obj = _enemyObjectPool.GetPooledEnemy((EnemyObjectPool.EnemyType)randomEnemyTypeIndex);

            //todo: There is no EnemyBaseClass. Therefore I get overall difficulty from EnemyMovement class. It needs to be refined.
            currentDifficultyLevel += obj.GetComponent<EnemyMovement>().enemyType.overallDifficulty;

            if (obj != null)
            {
                obj.transform.position = _spawnPosition;
                AliveEnemyCount++;
            }

            yield return new WaitForSeconds(config.spawnInterval);

            Debug.Log(currentDifficultyLevel);
            Debug.Log(config.totalDifficultyLevel);

        }
    }

    [System.Serializable]
    public class EnemySpawnConfig
    {
        [Header("Enemy Type That Will Spawn GreenEnemy/BlueEnemy/RedEnemy/WhiteEnemy")]
        public EnemyObjectPool.EnemyType enemyType;
        [Header("Enemy Count On Wave")]
        public int spawnCount;
        [Header("Enemies Spawn Interval")]
        public float spawnInterval;


        //Enemy Pooling
        public int totalDifficultyLevel;

    }
}