using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "stats/HealthData")]
public class HealthData : ScriptableObject
{
    public int redLimit = 6;
    public int currentRed = 3;
    public int currentSoul = 3;

    private int MaxHealth = 10;

    public bool AddHeart(HealthType healthType)
    {
        if (healthType == HealthType.Red)
        {
            if (currentRed < redLimit)
            {
                currentRed++;
                if (currentRed + currentSoul > MaxHealth)
                {
                    currentSoul = MaxHealth - currentRed;
                }

                return true;
            }
        }
        else if (healthType == HealthType.Soul)
        {
            if (currentRed + currentSoul < MaxHealth)
            {
                currentSoul++;
                return true;
            }
        }

        return false;
    }

    public bool RemoveHeart()
    {
        if (currentSoul > 0)
        {
            currentSoul--;
            return true;
        }
        else if (currentRed > 0)
        {
            currentRed--;
            return true;
        }

        return false;
    }
}