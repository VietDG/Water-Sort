using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterController : MonoBehaviour
{
    public Image _fillWater;

    private float _speed = 0.5f;
    public float Speed => _speed;

    private float _fill;
    public float Fill => _fill;

    public void Reset()
    {
    }

    private void Start()
    {
        _fillWater.fillAmount = 1;
    }

    public void TurnOffWave()
    {
        FillWave(0);
    }

    public void TurnOnWave()
    {
        FillWave(1);
    }

    public void FillWave(int value)
    {
        _fillWater.DOFillAmount(value, Speed);
    }
}
