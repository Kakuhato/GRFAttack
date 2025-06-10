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

        modifier.OnDispose += _ =>
        {
            modifiers.Remove(modifier);
            Queries -= modifier.Handle;
        };
    }

    public void Update(float deltaTime)
    {
        var node = modifiers.First;
        while (node != null)
        {
            var modifier = node.Value;
            modifier.Update(deltaTime);
            node = node.Next;
        }

        node = modifiers.First;
        while (node != null)
        {
            var nextNode = node.Next;
            if (node.Value.MarkedForRemoval)
            {
                node.Value.Dispose();
            }

            node = nextNode;
        }

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