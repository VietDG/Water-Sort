using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collection Datas", menuName = "Data SO/Skin Data", order = 1)]
public class CollectionSkinDataSO : ScriptableObject
{
    public List<DataItemSkin> dataItemSkins = new List<DataItemSkin>();
}
