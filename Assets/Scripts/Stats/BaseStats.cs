using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStats", menuName = "stats/BaseStats")]
public class BaseStats : ScriptableObject
{
    public int attack = 10;
    public int shootRange = 5;
    public int speed = 3;
    public int shootSpeed = 2;
}