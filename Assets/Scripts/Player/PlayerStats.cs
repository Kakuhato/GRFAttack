using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerStats : Entity
{
    public Health Health { get; private set; }

    [SerializeField, InlineEditor, Required]
    private HealthData healthData;

    void HandleTestEvent()
    {
        Debug.Log("TestEvnet received in PlayerStats");
    }

    protected override void Awake()
    {
        base.Awake();
        Health = new Health(healthData);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void TakeDamage(float damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            Health.RemoveHeart();
        }

        EventBus<HealthEvent>.Raise(new HealthEvent { hurts = (int)damage });
    }
}