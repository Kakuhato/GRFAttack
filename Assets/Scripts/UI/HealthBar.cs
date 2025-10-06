using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField, Required] private HealthPatten healthPatten;

    private GameObject heartItem;
    private LinkedList<GameObject> heartList = new LinkedList<GameObject>();
    private EventBinding<HealthEvent> healthEventBinding;
    private EventBinding<FreshHealthEvent> freshHealthEventBinding;


    private int HealthLimit = 10;

    public void ChangeHeart(HealthEvent healthEvent)
    {
        if (healthEvent.idx != GameManager.Instance.PlayerTransform) return;

        if (healthEvent.hurts > 0)
        {
            RemoveHealth();
        }
        else if (healthEvent.hurts < 0)
        {
            AddHealth(healthEvent.healthType);
        }
    }


    private void OnEnable()
    {
        healthEventBinding = new EventBinding<HealthEvent>(ChangeHeart);
        EventBus<HealthEvent>.Register(healthEventBinding);

        freshHealthEventBinding = new EventBinding<FreshHealthEvent>(Draw);
        EventBus<FreshHealthEvent>.Register(freshHealthEventBinding);
    }

    private void OnDisable()
    {
        EventBus<HealthEvent>.Unregister(healthEventBinding);
        EventBus<FreshHealthEvent>.Unregister(freshHealthEventBinding);
    }

    public void AddHealth(HealthType healthType)
    {
        if (healthType == HealthType.Red)
        {
            GameObject heart = healthPatten.CreateRed(heartItem, transform);
            heartList.AddFirst(heart);
            RemoveExtraHeart();
        }
        else if (healthType == HealthType.Soul)
        {
            GameObject heart = healthPatten.CreateSoul(heartItem, transform);
            heartList.AddLast(heart);
        }
    }

    public void RemoveHealth()
    {
        if (heartList.Count == 0) return;

        GameObject heart = heartList.Last.Value;
        heartList.RemoveLast();
        Destroy(heart);
    }


    // 删除多余的血量图标
    private void RemoveExtraHeart()
    {
        while (heartList.Count > HealthLimit)
        {
            GameObject heart = heartList.Last.Value;
            heartList.RemoveLast();
            Destroy(heart);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (healthPatten == null)
        {
            Debug.LogError("HealthData is not assigned in the inspector.");
            return;
        }
    }

    public void Draw(FreshHealthEvent freshHealthEvent)
    {
        // int redCount = GameManager.Instance.playerHealthInfo[0];
        // int soulCount = GameManager.Instance.playerHealthInfo[1];

        int redCount = freshHealthEvent.red;
        int soulCount = freshHealthEvent.soul;

        // Debug.Log(" Draw HealthBar: " + redCount + ", " + soulCount);

        foreach (var heart in heartList)
        {
            Destroy(heart);
        }

        heartList.Clear();

        heartItem = Resources.Load<GameObject>("UI/Heart");
        for (int i = 0; i < redCount; i++)
        {
            // TODO: 这部分逻辑需要抽出
            GameObject heart = healthPatten.CreateRed(heartItem, transform);
            heartList.AddLast(heart);
        }

        for (int i = 0; i < soulCount; i++)
        {
            GameObject heart = healthPatten.CreateSoul(heartItem, transform);
            heartList.AddLast(heart);
        }

        RemoveExtraHeart();
    }

    // Update is called once per frame
    void Update()
    {
    }
}