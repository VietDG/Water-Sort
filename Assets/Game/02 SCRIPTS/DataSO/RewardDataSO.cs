using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reward Datas", menuName = "Data SO/Reward Datas")]
public class RewardDataSO : ScriptableObject
{
    public List<int> ProcessValue = new List<int>();

    public int getValueMax(int id)
    {
        return ProcessValue[id];
    }

    public int MaxProcess()
    {
        return ProcessValue.Count - 1;
    }
}
