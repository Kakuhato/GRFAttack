using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    private readonly HealthData healthData;

    public Health(HealthData healthData)
    {
        this.healthData = healthData;
    }

    public bool AddHeart(HealthType healthType)
    {
        if (healthType == HealthType.Red)
        {
            if (this.healthData.currentRed < this.healthData.redLimit)
            {
                this.healthData.currentRed++;
                if (this.healthData.currentRed + this.healthData.currentSoul > HealthData.MAX_HEATH)
                {
                    this.healthData.currentSoul = HealthData.MAX_HEATH - this.healthData.currentRed;
                }

                return true;
            }
        }
        else if (healthType == HealthType.Soul)
        {
            if (this.healthData.currentRed + this.healthData.currentSoul < HealthData.MAX_HEATH)
            {
                this.healthData.currentSoul++;
                return true;
            }
        }

        return false;
    }

    public bool RemoveHeart()
    {
        if (this.healthData.currentSoul > 0)
        {
            this.healthData.currentSoul--;
            return true;
        }
        else if (this.healthData.currentRed > 0)
        {
            this.healthData.currentRed--;
            return true;
        }

        return false;
    }

    public int GetCurrentRed()
    {
        return this.healthData.currentRed;
    }

    public int GetCurrentSoul()
    {
        return this.healthData.currentSoul;
    }
}