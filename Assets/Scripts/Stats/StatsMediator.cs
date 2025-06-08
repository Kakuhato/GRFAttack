using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMediator
{
    //readonly防止modifiers指向另一个对象
    private readonly LinkedList<StatModifier> modifiers = new();

    public event EventHandler<Query> Queries;
    public void PerformerQuery(object sender, Query query) => Queries?.Invoke(sender, query);
    
    public void AddModifier(StatModifier modifier)
    {
        modifiers.AddLast(modifier);
        Queries += modifier.Handle;
    }

}

public class Query
{
    public readonly StatsType StatsType;
    public int Value;
    
    public Query(StatsType statsType, int value)
    {
        StatsType = statsType;
        Value = value;
    }
}