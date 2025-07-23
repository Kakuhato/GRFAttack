using UnityEngine;

public class HealthPickup : Pickup
{
    // TODO: 根据healthAmount循环多次扣血
    [SerializeField] private int healthAmount = 1;
    [SerializeField] private HealthType healthType = HealthType.Red;

    protected override bool ApplyPickupEffect(Entity entity)
    {
        bool success = entity.GetHealed(this.healthType);
        if (success)
        {
            GamePanel gamePanel = UIManager.Instance.GetPanel<GamePanel>();
            gamePanel.healthBar.AddHealth(this.healthType);
        }

        return success;
    }
}