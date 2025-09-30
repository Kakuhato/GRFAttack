using System;
using UnityEngine;

public abstract class BuffBase : PoolableObject, IVisitor
{
    protected abstract bool ApplyPickupEffect(Entity entity);

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is Entity entity)
        {
            if (ApplyPickupEffect(entity))
            {
                // 将对象返回对象池或者直接销毁
                Dispose();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IVisitable>()?.Accept(this);
        }
    }
}