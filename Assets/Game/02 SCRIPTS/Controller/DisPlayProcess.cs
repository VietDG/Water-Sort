using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayProcess : MonoBehaviour
{
    [SerializeField] Slider _process;
    [SerializeField] TMP_Text _ValueTxt;
    [SerializeField] TMP_Text _processTxt;

    private float _valueMin, _valueMax;
    [SerializeField] private float _duration = 0.5f;

    public float ValueMin
    {
        get { return _valueMin; }
        set
        {
            _valueMin = value;
            _process.value = (float)value / _valueMax;
            // Debug.LogError(value);
        }
    }

    public float ValueMax
    {
        get { return _valueMax; }
        set
        {
            _valueMax = value;
        }
    }

    public void UpdateProcess(int valueMin, int valueMax)
    {
        this.ValueMax = (float)valueMax;
        this.ValueMin = valueMin;
        //   _process.value = (float)valueMin / valueMax;
        _ValueTxt.text = $"{valueMin}/{valueMax}";
        // Debug.LogError($"Min:{valueMin}/ Max: {valueMax}");
    }

    public void UpdateInfor(string value)
    {
        _processTxt.text = value;
    }

    public void CallProcess(int valueMin, Action<bool> callBack)
    {
        FunctionCommon.ChangeValueInt((int)_valueMin, valueMin, _duration, 0, (result) =>
        {
            _ValueTxt.text = $"{result}/{this.ValueMax}";
        });

        _process.DOValue((float)valueMin / this.ValueMax, _duration).OnComplete(() =>
        {
            if (valueMin == this.ValueMax)
            {
                callBack?.Invoke(true);
            }
            else
            {
                callBack?.Invoke(false);
            }
        });
        // Debug.LogError($"Min:{valueMin}/ Max: {this.ValueMax}");
    }

    public void ResetProces()
    {
        _process.value = 0;
    }

    public bool isMaxProcess(int valueMin)
    {
        return valueMin == this.ValueMax;
    }
}
