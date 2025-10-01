using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "buff/BuffData")]
public class BuffData : ScriptableObject
{
    public StatsType type = StatsType.Attack;
    public OperatorType operatorType = OperatorType.Add;
    public int value = 10;
    public float duration = 5f;
    public Sprite icon;
}

public enum OperatorType
{
    Add,
    Multiply
}