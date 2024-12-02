//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BulletObjectPool : MonoBehaviour
//{
//    public static BulletObjectPool Instance { get; private set; }

//    [SerializeField] private BulletPool[] _pools = null;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }

//        InitializePools();
//    }

//    private void InitializePools()
//    {
//        for (int j = 0; j < _pools.Length; j++)
//        {
//            _pools[j].pooledBullets = new Queue<GameObject>();

//            for (int i = 0; i < _pools[j].pooledBulletSize; i++)
//            {
//                GameObject obj = Instantiate(_pools[j].bulletPrefab);
//                obj.SetActive(false);

//                _pools[j].pooledBullets.Enqueue(obj);
//            }
//        }
//    }

//    public GameObject GetPooledBullet(BulletType bulletType)
//    {
//        int bulletTypeIndex = (int)bulletType;

//        if (bulletTypeIndex >= _pools.Length)
//        {
//            return null;
//        }

//        GameObject obj = _pools[bulletTypeIndex].pooledBullets.Dequeue();
//        obj.SetActive(true);

//        _pools[bulletTypeIndex].pooledBullets.Enqueue(obj);

//        return obj;
//    }

//    [System.Serializable]
//    public class BulletPool
//    {
//        public Queue<GameObject> pooledBullets;
//        public GameObject bulletPrefab;
//        public int pooledBulletSize;
//    }

//    public enum BulletType
//    {
//        StandartBullet,
//        AOEBullet,
//        TeslaBullet,
//        ElectricBullet,
//        MortarBullet,
//    }
//}

using UnityEngine;
using System.Collections.Generic;

public class BulletObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class BulletPoolItem
    {
        public GameObject prefab;
        public int poolSize;
        public BulletType type;
        public List<GameObject> pooledObjects = new List<GameObject>();
    }

    [SerializeField] private BulletPoolItem[] bulletPools;

    private Dictionary<BulletType, BulletPoolItem> poolDictionary = new Dictionary<BulletType, BulletPoolItem>();

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var pool in bulletPools)
        {
            pool.pooledObjects.Clear();
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                pool.pooledObjects.Add(obj);
            }

            poolDictionary[pool.type] = pool;
        }
    }

    public GameObject GetPooledBullet(BulletType bulletType)
    {
        if (!poolDictionary.TryGetValue(bulletType, out BulletPoolItem poolItem))
        {
            Debug.LogError($"No pool found for bullet type: {bulletType}");
            return null;
        }

        // Find first inactive bullet
        foreach (GameObject obj in poolItem.pooledObjects)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // If no inactive bullets, create new one
        GameObject newObj = Instantiate(poolItem.prefab);
        poolItem.pooledObjects.Add(newObj);
        return newObj;
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    public enum BulletType
    {
        DirectBullet,
        AOEBullet,
        TeslaBullet,
        ElectricBullet,
        MortarBullet
    }
}