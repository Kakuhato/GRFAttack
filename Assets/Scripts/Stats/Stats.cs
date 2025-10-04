using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsType
{
    Attack,
    ShootRange,
    Speed,
    ShootSpeed,
    Health
}

public class Stats
{
    private readonly BaseStats baseStats;
    private readonly StatsMediator mediator;

    public StatsMediator Mediator => mediator;

    public float Attack
    {
        get
        {
            var q = new Query(StatsType.Attack, baseStats.attack);
            mediator.PerformerQuery(this, q);
            return q.Value;
        }
    }

    public float ShootRange
    {
        get
        {
            var q = new Query(StatsType.ShootRange, baseStats.shootRange);
            mediator.PerformerQuery(this, q);
            return q.Value;
        }
    }

    public float Speed
    {
        get
        {
            var q = new Query(StatsType.Speed, baseStats.speed);
            mediator.PerformerQuery(this, q);
            return q.Value;
        }
    }

    public float ShootSpeed
    {
        get
        {
            var q = new Query(StatsType.ShootSpeed, baseStats.shootSpeed);
            mediator.PerformerQuery(this, q);
            return q.Value;
        }
    }

    public Stats(StatsMediator mediator, BaseStats baseStats)
    {
        this.mediator = mediator;
        this.baseStats = baseStats;
    }

    public PublishStats ToPublish()
    {
        return new PublishStats
        {
            Attack = (int)Attack,
            ShootRange = (int)ShootRange,
            Speed = (int)Speed,
            ShootSpeed = (int)ShootSpeed
        };
    }

    public override string ToString() =>
        $"Attack: {Attack}, ShootRange: {ShootRange}, Speed: {Speed}, ShootSpeed: {ShootSpeed}";
}