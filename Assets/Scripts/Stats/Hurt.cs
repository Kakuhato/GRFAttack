using UnityEngine;

public class Hurt : Pickup
{
    [SerializeField] private int hurtAmount = 1;

    protected override bool ApplyPickupEffect(Entity entity)
    {
        bool success = entity.GetDamaged();
        if (success)
        {
            GamePanel gamePanel = UIManager.Instance.GetPanel<GamePanel>();
            gamePanel.healthBar.RemoveHealth();
        }
        else
        {
            GameManager.Instance.GameOver();
        }

        return success;
    }
}