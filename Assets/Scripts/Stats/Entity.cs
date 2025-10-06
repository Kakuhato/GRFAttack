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

    public virtual bool IsDead { get; set; } = false;
    public event Action OnDeath = delegate { };
    public event Action OnRevive = delegate { };

    protected void RaiseOnDeath() => OnDeath?.Invoke();
    protected void RaiseOnRevive() => OnRevive?.Invoke();


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

    public virtual void Die()
    {
    }

    public virtual void Revive()
    {
        // IsDead = false;
    }

    public virtual List<int> GetHealthInfo()
    {
        return new List<int>(2);
    }
}