using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    private int redLimit = 6;
    private int currentRed = 3;
    private int currentSoul = 3;

    public Health(HealthData healthData)
    {
        Init(healthData);
    }

    public bool AddHeart(HealthType healthType)
    {
        if (healthType == HealthType.Red)
        {
            if (this.currentRed < this.redLimit)
            {
                this.currentRed++;
                if (this.currentRed + this.currentSoul > HealthData.MAX_HEATH)
                {
                    this.currentSoul = HealthData.MAX_HEATH - this.currentRed;
                }

                return true;
            }
        }
        else if (healthType == HealthType.Soul)
        {
            if (this.currentRed + this.currentSoul < HealthData.MAX_HEATH)
            {
                this.currentSoul++;
                return true;
            }
        }

        return false;
    }

    public bool RemoveHeart()
    {
        if (this.currentSoul > 0)
        {
            this.currentSoul--;
            return true;
        }
        else if (this.currentRed > 0)
        {
            this.currentRed--;
            return true;
        }

        return false;
    }

    public int GetCurrentRed()
    {
        return this.currentRed;
    }

    public int GetCurrentSoul()
    {
        return this.currentSoul;
    }

    public void Init(HealthData healthData)
    {
        Debug.Log("Init Health: " + healthData.redLimit + ", " + healthData.currentRed + ", " + healthData.currentSoul);
        this.redLimit = healthData.redLimit;
        this.currentRed = healthData.currentRed;
        this.currentSoul = healthData.currentSoul;
    }
}