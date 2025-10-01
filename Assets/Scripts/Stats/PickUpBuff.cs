using System;
using UnityEngine;
using Utils;


public class PickUpBuff : BuffBase
{
    [SerializeField] BuffData buffData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = this.gameObject.GetOrAdd<SpriteRenderer>();
    }

    public override void Init(BuffData data)
    {
        buffData = data;
        spriteRenderer.sprite = buffData.icon;
    }

    protected override bool ApplyPickupEffect(Entity entity)
    {
        if (buffData.type == StatsType.Health)
        {
            return entity.Heal(buffData.value);
        }

        StatModifier modifier = buffData.operatorType switch
        {
            OperatorType.Add => new BasicStatModifier(buffData.type, buffData.duration, v => v + buffData.value),
            OperatorType.Multiply => new BasicStatModifier(buffData.type, buffData.duration, v => v * buffData.value),
            _ => throw new ArgumentOutOfRangeException()
        };

        entity.Stats.Mediator.AddModifier(modifier);

        return true;
    }
}