using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsType
{
    Attack,
    Defence,
}

public class Stats
{
    private readonly BaseStats baseStats;
    
    public int Attack => baseStats.attack;
    
    public int Defence => baseStats.defence;
}