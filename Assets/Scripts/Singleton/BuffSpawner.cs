using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class BuffSpawner : RegulatorSingleton<BuffSpawner>
{
    [SerializeField] private BuffDropTable buffDropTable;
    [SerializeField] private GameObject buffPrefab;

    private EventBinding<EnemyDieEvent> enemyDieEventBinding;

    public void SpawnBuff(EnemyDieEvent data)
    {
        if (buffDropTable.buffDropRates.Count == 0)
        {
            Debug.LogWarning("Buff drop table is empty!");
            return;
        }

        BuffData selectedBuff = GetRandomBuff();
        Debug.Log("Spawning buff: " + selectedBuff.type);
        GameObject buffObject = PoolManager.Instance.SpawnObject(buffPrefab, data.Position, PoolType.Buff);
        buffObject.GetComponent<BuffBase>().Init(selectedBuff);
        buffObject.SetActive(true);
    }

    private BuffData GetRandomBuff()
    {
        float totalWeight = 0f;
        foreach (var buff in buffDropTable.buffDropRates)
        {
            totalWeight += buff.weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var buff in buffDropTable.buffDropRates)
        {
            cumulativeWeight += buff.weight;
            if (randomValue <= cumulativeWeight)
            {
                return buff.buffData;
            }
        }

        return null; // Should never reach here if weights are set correctly
    }

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        if (buffDropTable == null)
        {
            buffDropTable = Resources.Load<BuffDropTable>("Datas/Buff/DefaultTable");
            if (buffDropTable == null)
                Debug.LogError("Buff drop table not found in Resources/Datas/Buff/DefaultTable");
        }

        if (buffPrefab == null)
        {
            buffPrefab = Resources.Load<GameObject>("Prefabs/PickUpBuff");
            if (buffPrefab == null)
                Debug.LogError("Buff prefab not found in Resources/Prefabs/PickUpBuff");
        }

        enemyDieEventBinding = new EventBinding<EnemyDieEvent>(SpawnBuff);
        EventBus<EnemyDieEvent>.Register(enemyDieEventBinding);
    }

    private void OnDestroy()
    {
        EventBus<EnemyDieEvent>.Unregister(enemyDieEventBinding);
    }
}