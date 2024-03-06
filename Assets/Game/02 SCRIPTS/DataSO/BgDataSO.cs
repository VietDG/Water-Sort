using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ball Datas", menuName = "Data SO/BG Datas")]
public class BgDataSO : ScriptableObject
{
    public List<Sprite> _topList;

    public Sprite getBG(int index)
    {
        return _topList[index];
    }
}

