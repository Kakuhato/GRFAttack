using UnityEngine;

public abstract class Pickup : MonoBehaviour, IVisitor
{
    protected abstract void ApplyPickupEffect(Entity entity);

    public void Visit<T>(T visitable) where T : Component, Ivisitable
    {
        if (visitable is Entity entity)
        {
            ApplyPickupEffect(entity);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Ivisitable>()?.Accept(this);
            // 将对象返回对象池或者直接销毁
            Destroy(this.gameObject);
        }
    }
}
