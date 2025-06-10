using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsType
{
    Attack,
    Defence,
}

public class Stats
{
    private readonly BaseStats baseStats;
    private readonly StatsMediator mediator;
    
    public StatsMediator Mediator => mediator;
    
    public int Attack
    {
        get
        {
            var q = new Query(StatsType.Attack, baseStats.attack);
            mediator.PerformerQuery(this, q);
            return q.Value;
        }
    }

    public int Defence
    {
        get
        {
            var q = new Query(StatsType.Defence, baseStats.defence);
            mediator.PerformerQuery(this, q);
            return q.Value;
        }
    }
    
    public Stats(StatsMediator mediator, BaseStats baseStats)
    {
        this.mediator = mediator;
        this.baseStats = baseStats;
    }

    public override string ToString() => $"Attack: {Attack}, Defence: {Defence}";
}