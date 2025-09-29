using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using Random = UnityEngine.Random;

public class EnemySpawner : RegulatorSingleton<EnemySpawner>
{
    [SerializeField] private bool isInBattle = false;
    private Tilemap spawnGround;
    private Tilemap foreGround;
    private GameObject prefab;
    private CountdownTimer spawnTimer;

    [SerializeField] private int maxSpawnAttempts = 10;
    [SerializeField] private float minRadius = 10f;
    [SerializeField] private float maxRadius = 12f;
    [SerializeField] private int maxEnemies = 30;
    [SerializeField] private int currentEnemies = 0;
    [SerializeField] private float spawnInterval = 1f;

    public void StartSpawning()
    {
        GetTilemaps();
        isInBattle = true;
    }

    public void StopSpawning()
    {
        currentEnemies = 0;
        isInBattle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInBattle)
        {
            spawnTimer.Tick(Time.deltaTime);
        }
    }

    private void TrySpawnEnemy()
    {
        if (currentEnemies >= maxEnemies) return;
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            float radus = Random.Range(minRadius, maxRadius);
            float angle = Random.Range(0, 2 * Mathf.PI);
            Vector3 randomPosition = GameManager.Instance.PlayerTransform.position +
                                     new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radus;

            Vector3Int cellPosition = spawnGround.WorldToCell(randomPosition);
            if (spawnGround.HasTile(cellPosition))
            {
                if (!Physics2D.OverlapCircle(randomPosition, 0.5f,
                        LayerMask.GetMask("Foreground", "Enemy", "Player")) &&
                    !Physics2D.CircleCast(randomPosition, 0.2f,
                        (GameManager.Instance.PlayerTransform.position - randomPosition).normalized,
                        Vector2.Distance(GameManager.Instance.PlayerTransform.position, randomPosition),
                        LayerMask.GetMask("Foreground")))
                {
                    GameObject enemy = PoolManager.Instance.SpawnObject(prefab, randomPosition,
                        new List<Action<Poolable>>
                        {
                            (_) =>
                            {
                                currentEnemies--;
                                TrySpawnEnemy();
                                print("spanw1");
                                TrySpawnEnemy();
                                print("spanw2");
                                GameManager.Instance.AddScore(1);
                            }
                        }, PoolType.Enemy);
                    enemy.SetActive(true);
                    print("spanworiginal");
                    currentEnemies++;
                    return;
                }
            }
        }

        Debug.LogWarning("Failed to find a valid spawn position after maximum attempts.");
    }


    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        GetTilemaps();
        if (spawnGround == null || foreGround == null)
        {
            Debug.LogError("SpawnGround or ForeGround Tilemap not found!");
        }

        prefab = Resources.Load<GameObject>("Prefabs/Enemy");
        if (prefab == null)
        {
            Debug.LogError("Enemy prefab not found in Resources/Prefabs!");
        }

        spawnTimer = new CountdownTimer(spawnInterval);
        spawnTimer.OnTimerStop += () => spawnTimer.Start();
        spawnTimer.OnTimerStart += TrySpawnEnemy;
        spawnTimer.Start();
    }

    void GetTilemaps()
    {
        if (spawnGround == null)
        {
            spawnGround = GameObject.FindWithTag("SpawnGround").GetComponent<Tilemap>();
            if (spawnGround == null)
            {
                Debug.LogError("SpawnGround Tilemap not found!");
            }
        }

        if (foreGround == null)
        {
            foreGround = GameObject.FindWithTag("ForeGround").GetComponent<Tilemap>();
            if (foreGround == null)
            {
                Debug.LogError("ForeGround Tilemap not found!");
            }
        }
    }
}