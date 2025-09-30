using UnityEngine;

public class HealthBuffBase : BuffBase
{
    // TODO: 根据healthAmount循环多次扣血
    [SerializeField] private int healthAmount = 1;
    [SerializeField] private HealthType healthType = HealthType.Red;

    protected override bool ApplyPickupEffect(Entity entity)
    {
        // bool success = entity.Health.AddHeart(this.healthType);
        // if (success)
        // {
        //     GamePanel gamePanel = UIManager.Instance.GetPanel<GamePanel>();
        //     gamePanel.healthBar.AddHealth(this.healthType);
        // }

        return true;
    }
}