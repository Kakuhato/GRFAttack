using System;
using UnityEngine;

public abstract class Pickup : MonoBehaviour, IVisitor
{
    protected abstract bool ApplyPickupEffect(Entity entity);

    public void Visit<T>(T visitable) where T : Component, Ivisitable
    {
        if (visitable is Entity entity)
        {
            if (ApplyPickupEffect(entity))
            {
                // 将对象返回对象池或者直接销毁
                Destroy(this.gameObject);
            }
        }
    }

    // 之后改成实体后，Trigger需要修改
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Ivisitable>()?.Accept(this);
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Ivisitable>()?.Accept(this);
        }
    }
}