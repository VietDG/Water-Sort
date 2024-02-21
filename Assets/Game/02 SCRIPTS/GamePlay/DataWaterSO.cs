using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaterData", menuName = "Data SO/Water Data")]
public class DataWaterSO : ScriptableObject
{
    public List<WaterData> waterList = new List<WaterData>();

    public WaterData GetWaterData(int index)
    {
        return waterList[index];
    }
}
