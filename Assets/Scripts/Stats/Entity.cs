using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Entity : MonoBehaviour, Ivisitable
{
    [SerializeField, InlineEditor, Required]
    private BaseStats baseStats;

    public Stats Stats { get; private set; }


    protected virtual void Awake()
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

    public virtual void TakeDamage(float damage)
    {
        // TODO: 将敌人逻辑和玩家逻辑统一继承到这里
    }
}