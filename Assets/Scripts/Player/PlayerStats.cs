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

    protected override void Awake()
    {
        base.Awake();
        Health = new Health(healthData);
    }


    public override void TakeDamage(float damage, Vector2 direction)
    {
        for (int i = 1; i <= damage; i++)
        {
            Health.RemoveHeart();

            EventBus<HealthEvent>.Raise(new HealthEvent { hurts = 1, idx = this.transform });

            if (Health.GetCurrentRed() <= 0) // TODO: 修改血量逻辑
            {
                Die();
                EventBus<PlayerDieEvent>.Raise(new PlayerDieEvent { playerTransform = this.transform });
                break;
            }
        }
    }


    public override bool Heal(float amount)
    {
        if (Health.AddHeart(HealthType.Soul))
        {
            EventBus<HealthEvent>.Raise(new HealthEvent
                { healthType = HealthType.Soul, hurts = -1, idx = this.transform });
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Die()
    {
        if (IsDead) return;
        IsDead = true;
        RaiseOnDeath();
    }

    public override void Revive()
    {
        if (!IsDead) return;
        IsDead = false;
        Health.Init(healthData);
        RaiseOnRevive();
    }

    public override List<int> GetHealthInfo()
    {
        Debug.Log("GetHealthInfo: " + Health.GetCurrentRed() + ", " + Health.GetCurrentSoul());
        return new List<int> { Health.GetCurrentRed(), Health.GetCurrentSoul() };
    }
}