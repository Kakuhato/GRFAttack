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

    // TODO: 换成公共函数获取初始生命
    public HealthData Health => healthData;

    public Stats Stats { get; private set; }

    private void Awake()
    {
        Stats = new Stats(new StatsMediator(), baseStats);
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

    public bool GetHealed(HealthType healthType)
    {
        return this.healthData.AddHeart(healthType);
    }

    public bool GetDamaged()
    {
        return this.healthData.RemoveHeart();
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