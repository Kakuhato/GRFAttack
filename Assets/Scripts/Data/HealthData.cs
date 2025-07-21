using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HealthData", menuName = "ScriptableObjects/HealthData")]
public class HealthData : ScriptableObject
{
    public Sprite redHeart;
    public Sprite soulHeart;
    public int initialHealth;
}
