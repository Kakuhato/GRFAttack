using UnityEngine;

public class Hurt : Pickup
{
    [SerializeField] private int hurtAmount = 1;

    protected override bool ApplyPickupEffect(Entity entity)
    {
        entity.TakeDamage(hurtAmount);

        return true;
    }
}