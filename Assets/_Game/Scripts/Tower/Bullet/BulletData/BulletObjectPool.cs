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
        MortarBullet,
        LaserBullet
    }
}