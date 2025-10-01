using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffDropRate
{
    public BuffData buffData;
    public float weight;
}


[CreateAssetMenu(fileName = "BuffDropTable", menuName = "buff/BuffDropTable")]
public class BuffDropTable : ScriptableObject
{
    public List<BuffDropRate> buffDropRates;
}