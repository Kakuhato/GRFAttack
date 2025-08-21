using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Entity : MonoBehaviour, Ivisitable
{
    [SerializeField, InlineEditor, Required]
    private BaseStats baseStats;

    [SerializeField, InlineEditor, Required]
    private HealthData healthData;


    public Stats Stats { get; private set; }

    public Health Health { get; private set; }

    private void Awake()
    {
        Stats = new Stats(new StatsMediator(), baseStats);
        Health = new Health(healthData);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Tick();
    }

    public void Accept(IVisitor visitor) => visitor.Visit(this);

    public void Tick()
    {
        Stats.Mediator.Update(Time.deltaTime);
    }

    public void Equip(IEquipable item)
    {
        if (item == null) return;
        Stats.Mediator.AddModifier(item.GetStatModifier());
    }

    public void Unequip(IEquipable item)
    {
        if (item == null) return;
        item.GetStatModifier().Remove();
    }
}

public interface IEquipable
{
    StatModifier GetStatModifier();
}

public class Sword : IEquipable
{
    public StatModifier GetStatModifier()
    {
        return new BasicStatModifier(StatsType.Attack, 0, v => v + 10);
    }
}