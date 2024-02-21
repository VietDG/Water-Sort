using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public WaterData data;
    [SerializeField] Color color;
    public int Id => data.index;

    public void Init(WaterData data)
    {
        this.data = data;
        this.color = data.color;
    }
}
