using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ball Datas", menuName = "Data SO/Top Datas")]
public class TopDataSO : ScriptableObject
{
    public List<Sprite> _topList;

    public Sprite getTop(int index)
    {
        return _topList[index];
    }
}

