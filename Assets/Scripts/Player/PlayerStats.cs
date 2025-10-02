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
            EventBus<HealthEvent>.Raise(new HealthEvent { hurts = 1 });

            if (Health.GetCurrentRed() <= 0)
            {
                Die();
                break;
            }
        }
    }

    public override bool Heal(float amount)
    {
        if (Health.AddHeart(HealthType.Soul))
        {
            EventBus<HealthEvent>.Raise(new HealthEvent { healthType = HealthType.Soul, hurts = -1 });
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Die()
    {
        // EventBus<GameOverEvent>.Raise(new GameOverEvent());
    }

    public override List<int> GetHealthInfo()
    {
        return new List<int> { Health.GetCurrentRed(), Health.GetCurrentSoul() };
    }
}