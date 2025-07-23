using System;
using UnityEngine;

public enum OperatorType
{
    Add,
    Multiply
}

public class StatModifierPickup : Pickup
{
    [SerializeField] private StatsType type = StatsType.Attack;
    [SerializeField] private OperatorType operatorType = OperatorType.Add;
    [SerializeField] private int value = 10;
    [SerializeField] private float duration = 5f;

    protected override bool ApplyPickupEffect(Entity entity)
    {
        StatModifier modifier = operatorType switch
        {
            OperatorType.Add => new BasicStatModifier(type, duration, v => v + value),
            OperatorType.Multiply => new BasicStatModifier(type, duration, v => v * value),
            _ => throw new ArgumentOutOfRangeException()
        };

        entity.Stats.Mediator.AddModifier(modifier);

        return true;
    }
}