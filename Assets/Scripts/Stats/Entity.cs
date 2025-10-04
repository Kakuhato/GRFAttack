using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Entity : MonoBehaviour, IVisitable
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

    public virtual void TakeDamage(float damage, Vector2 direction)
    {
        // TODO: 不要传入方向，传入打击者的位置和力度
    }

    public virtual bool Heal(float amount)
    {
        return false;
    }

    public virtual void KickBack(Vector2 direction)
    {
        // TODO: 不要传入方向，传入打击者的位置和力度
    }

    public virtual List<int> GetHealthInfo()
    {
        return new List<int>();
    }
}