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


    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        bulletPool = new GameObject("BulletPool");
        // bulletPool.transform.parent = this.transform;
        enemyPool = new GameObject("EnemyPool");
        // enemyPool.transform.parent = this.transform;
    }

    public GameObject SpawnObject(
        GameObject prefab,
        Vector3 position,
        List<Action<Poolable>> onDespawn,
        PoolType type = PoolType.None
    )
    {
        if (prefab.GetComponent<Poolable>() == null)
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
            Poolable poolable = spawnedObject.GetComponent<Poolable>();
            poolable.OnDispose += DespawnObject;
            foreach (var func in onDespawn)
            {
                poolable.OnDispose += func;
            }

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
        return SpawnObject(prefab, position, new List<Action<Poolable>>(), type);
    }


    public void DespawnObject(Poolable obj)
    {
        // 这里返回的是场景上的对象，而非脚本，因此会带有(Clone)
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
        switch (type)
        {
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