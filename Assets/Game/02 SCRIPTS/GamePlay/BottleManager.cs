using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleManager : SingletonMonoBehaviour<BottleManager>
{
    [SerializeField] GameObject _waterPrefab;
    [SerializeField] float _spaceHorizontal, _spaceVertical, _tubeHorizontalMax;
    public List<BottleController> bottleList = new List<BottleController>();
    public BottleController _holdingBottle;



}
