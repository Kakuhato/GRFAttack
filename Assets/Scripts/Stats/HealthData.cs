using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "stats/HealthData")]
public class HealthData : ScriptableObject
{
    public int redLimit = 6;
    public int currentRed = 3;
    public int currentSoul = 3;
    public const int MAX_HEATH = 10;
}