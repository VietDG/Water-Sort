using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubeController : MonoBehaviour
{
    [Header("Data")]
    public WaterData waterData;
    public DataBottle dataBottle;
    [Header("REFERENCE")]
    public Image[] _waterImg;
    public List<WaterController> _waterList = new List<WaterController>();
    [Header("VALUE")]
    private int _slot;
    public int SLot => _slot;

    private int _fillAmout;
    public int FillAMmout => _fillAmout;

    private Vector2 _startPosMove;
    public Vector2 StartPosMove => _startPosMove;

    private float _duration = 0.5f;
    public void Init(DataBottle dataBottle, int value)
    {
        this.dataBottle = dataBottle;
        this.dataBottle.slot = value;
    }

    private void Start()
    {
        //_fillWater.fillAmount = 0;
    }

    public void StartMove(TubeController tube, bool value, float originalChildCout = 0, int index = 0)
    {
        if (value)
        {
            // this.transform.DOMove
        }
        else
        {

        }
    }

    public void SetPosition(Vector2 target)
    {
        this.transform.position = target;
        //  _startPosMove = _
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AppenOff(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            // _waterList[0].TurnOffWave();
            AppenWater(0);
        }
    }

    public void AppenWater(int value)
    {
        var sequence = DOTween.Sequence();

        var t1 = _waterList[0]._fillWater.DOFillAmount(value, _duration);
        var t2 = _waterList[1]._fillWater.DOFillAmount(value, _duration);


        sequence.Append(t1);
        sequence.Append(t2);
    }

    public void AppenOff(int value)
    {
        var sequence = DOTween.Sequence();

        var t1 = _waterList[0]._fillWater.DOFillAmount(value, _duration).SetEase(Ease.InQuad);
        var t2 = _waterList[1]._fillWater.DOFillAmount(value, _duration).SetEase(Ease.InQuad);


        sequence.Append(t2);
        sequence.Append(t1);
    }

}
