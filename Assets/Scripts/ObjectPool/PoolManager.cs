using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PoolManager : RegulatorSingleton<PoolManager>
{
    private List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

    private GameObject bulletPool;
    private GameObject enemyPool;
    private GameObject buffPool;


    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        GetPool();
    }

    public void GetPool()
    {
        if (buffPool == null)
            bulletPool = new GameObject("BulletPool");
        if (enemyPool == null)
            enemyPool = new GameObject("EnemyPool");
        if (buffPool == null)
            buffPool = new GameObject("BuffPool");
    }

    public GameObject SpawnObject(
        GameObject prefab,
        Vector3 position,
        Action<IPoolable> onDespawn,
        PoolType type = PoolType.None
    )
    {
        if (prefab.GetComponent<IPoolable>() == null)
        {
            Debug.LogWarning("Trying to spawn an object that is not poolable: " + prefab.name);
            return null;
        }

        PooledObjectInfo pool = objectPools.Find(p => p.PoolName == prefab.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { PoolName = prefab.name };
            objectPools.Add(pool);
        }

        GameObject spawnedObject = null;
        
        pool.PooledObjects.RemoveAll(item => item == null);
        foreach (var item in pool.PooledObjects)
        {
            if (item != null)
            {
                spawnedObject = item;
                break;
            }
        }

        if (spawnedObject == null)
        {
            GameObject parentPool = GetParentPool(type);
            spawnedObject = Instantiate(prefab, position, Quaternion.identity);
            spawnedObject.SetActive(false);
            IPoolable poolable = spawnedObject.GetComponent<IPoolable>();
            poolable.OnDispose += onDespawn;
            poolable.OnDispose += DespawnObject;

            if (parentPool != null)
                spawnedObject.transform.parent = parentPool.transform;
        }
        else
        {
            pool.PooledObjects.Remove(spawnedObject);
            spawnedObject.transform.position = position;
            spawnedObject.transform.rotation = Quaternion.identity;
            // spawnedObject.SetActive(true);
        }

        return spawnedObject;
    }

    public GameObject SpawnObject(
        GameObject prefab,
        Vector3 position,
        PoolType type = PoolType.None
    )
    {
        return SpawnObject(prefab, position, null, type);
    }


    public void DespawnObject(IPoolable ip)
    {
        // 这里返回的是场景上的对象，而非脚本，因此会带有(Clone)
        var obj = (ip as Component);
        if (obj == null)
        {
            Debug.LogWarning("Trying to despawn an object that is not a component: " + ip.ToString());
            return;
        }

        PooledObjectInfo pool = objectPools.Find(p => p.PoolName == obj.gameObject.name.Replace("(Clone)", "").Trim());

        if (pool == null)
        {
            Debug.LogWarning("Trying to despawn an object that was not spawned from the pool: " + obj.gameObject.name);
        }
        else
        {
            obj.gameObject.SetActive(false);
            // print("despawned: " + obj.name);
            pool.PooledObjects.Add(obj.gameObject);
        }
    }


    private GameObject GetParentPool(PoolType type)
    {
        GetPool();
        switch (type)
        {
            case PoolType.Buff:
                return buffPool;
            case PoolType.Bullet:
                return bulletPool;
            case PoolType.Enemy:
                return enemyPool;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }
}


public class PooledObjectInfo
{
    public string PoolName;
    public List<GameObject> PooledObjects = new List<GameObject>();
}