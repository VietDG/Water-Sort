using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public enum ColorShader
{
    C1,
    C2,
    C3,
    C4,
}
[Serializable]
public class WaterData
{
    public int index;
    public Color color;
}
[Serializable]
public class DataBottle
{
    public int slot;
    public List<WaterData> waterDa = new List<WaterData>();

    public DataBottle(int slot, List<WaterData> color)
    {
        this.slot = slot;
        this.waterDa = color;
    }
    public DataBottle() { }
}

public class BottleController : MonoBehaviour
{
    [Header("----------------------------------REFERENCE--------------------------------")]
    public List<Color> bottleColors;
    public SpriteRenderer bottleMask;
    public SpriteRenderer _ava;

    public DataBottle datawaterColor;

    private int _slot;
    public int Slot => _slot;

    public LineRenderer _lineRenderer;

    public AnimationCurve scaleAndRotete;
    public AnimationCurve fillAmoutCurve;
    public AnimationCurve rotationSpeed;
    public float timeRotate;
    [Header("---------------------------------VALUE-----------------------------------")]
    public float[] fillAmouts;
    public float[] rotationValues;
    private int rotationIndex = 0;
    [Range(0, 4)]
    public int numberofCOlor = 4;
    public int numberofTOpColorLayers = 1;

    public GameObject _bottleTop;

    private int numberOfColorToTransfer = 0;

    public Transform leftRotationPoint;
    public Transform rightRotationPoint;
    private Transform choseRotationPoint;
    [SerializeField] Transform _spawnTrans;
    [SerializeField] Transform _startTeransMove;
    public Transform _linePos;

    public Transform _posS;

    public Transform roteleft, roteright;

    private float directionMultiple = 1f;

    Vector3 originalPosition;
    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 midPos;

    public float weight;
    public float height;

    private Vector2 _startPosMove;
    public Vector2 StartPosMove => _startPosMove;

    private Vector3 _leftPos;
    public Vector3 LeftPos => _leftPos;

    private Vector3 _rightPos;
    public Vector3 RightPos => _rightPos;

    private Vector3 _topPos;

    [SerializeField] ParticleSystem _vfx;

    private void Awake()
    {
        weight = _ava.bounds.size.x;
        height = _ava.bounds.size.y;
    }

    private void OnMouseDown()
    {
        GameController.Instance.OnClick(this);
    }

    public void Complete()
    {
        _bottleTop.gameObject.SetActive(true);
        _bottleTop.transform.DOMoveY(_bottleTop.transform.position.y - 0.1f, 0.2f).SetDelay(0.1f).SetEase(Ease.InQuad);

    }

    void Start()
    {

    }

    private void Reset()
    {
        this.datawaterColor = null;
    }


    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        Reset();
    }

    private void AddColor()
    {
        for (int i = 0; i < datawaterColor.waterDa.Count; i++)
        {
            bottleColors.Add(datawaterColor.waterDa[i].color);
        }
    }

    public bool isEmpty()
    {
        if (datawaterColor.waterDa.Count > 0)
        {
            return false;
        }
        return true;
    }

    public void HandleColor()
    {
        bottleMask.material.SetFloat("_FillAmout", fillAmouts[numberofCOlor]);
    }

    public void Init(DataBottle dataColor, int value)
    {
        datawaterColor = dataColor;
        dataColor.slot = value;
        this._slot = dataColor.slot;
        numberofCOlor = dataColor.waterDa.Count;
        AddColor();
        HandleColor();
        originalPosition = transform.position;
        _topPos = _bottleTop.transform.position;
        UpdateStartColor();
    }

    public void InitPos(Vector2 target, int slot)
    {
        SetPosition(target);
    }

    public void SetPosition(Vector2 target)
    {
        this.transform.position = target;
        _startPosMove = _spawnTrans.position;
    }

    public void StartColorTransfer(BottleController bottle)
    {
        ChoseRotationPointAndDirection(bottle);


        CaulateRoattionIndex(4 - bottle.numberofCOlor);

        _ava.sortingOrder += 2;
        bottleMask.sortingOrder += 2;
        Rote(bottle);
    }

    public string[] _colorName = new string[] { "C1", "C2", "C3", "C4" }; // Update Start Color
    public void UpdateStartColor()
    {
        for (int i = 0; i < datawaterColor.waterDa.Count; i++)
        {
            bottleMask.material.SetColor(_colorName[i], bottleColors[i]);
        }
    }

    private float _duration = 0.5f;

    public void Rote(BottleController bottle)
    {
        if (choseRotationPoint == leftRotationPoint)
        {
            endPosition = bottle.roteright.position;
        }
        else
        {
            endPosition = bottle.roteleft.position;
        }

        Vector2 target = new Vector2(endPosition.x/*+ directionMultiple*/, endPosition.y);

        this.transform.DOMove(target, 0.2f).SetEase(Ease.Linear).SetDelay(0.1f).OnComplete(() =>
        {
            bottleMask.material.DOFloat(0.15f, "_scale", _duration).SetEase(Ease.Linear);
            bottleMask.material.DOFloat(fillAmouts[datawaterColor.waterDa.Count], "_FillAmout", _duration).SetEase(Ease.Linear);

            bottle.bottleMask.material.DOFloat(bottle.fillAmouts[bottle.datawaterColor.waterDa.Count], "_FillAmout", _duration).SetEase(Ease.Linear);
            bottle.UpdateStartColor();

            FunctionCommon.DelayTime(_duration / 2, () =>
            {
                StartCoroutine(SetLine(bottle));
            });
            this.transform.DORotate(new Vector3(0, 0, directionMultiple * rotationValues[datawaterColor.waterDa.Count]), _duration).OnComplete(() =>
            {
                RoteBack(target, bottle);
                _lineRenderer.enabled = false;
                _lineRenderer.gameObject.SetActive(false);
            });
        });
    }

    public void SetColorBooster()
    {
        bottleColors.Clear();
        AddColor();
        bottleMask.material.SetFloat("_FillAmout", fillAmouts[datawaterColor.waterDa.Count]);
        _bottleTop.SetActive(false);
        _bottleTop.transform.position = _topPos;
    }


    IEnumerator SetLine(BottleController bottle)
    {
        float t = 0;
        _lineRenderer.gameObject.SetActive(true);
        _lineRenderer.startColor = bottle.bottleColors[^1];//
        _lineRenderer.endColor = bottle.bottleColors[^1];
        _lineRenderer.enabled = true;
        ChoseRotationPointAndDirection(bottle);
        while (t <= _duration)
        {
            _lineRenderer.SetPosition(0, choseRotationPoint.position);
            _lineRenderer.SetPosition(1, choseRotationPoint.position - Vector3.up * 0.7f);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _lineRenderer.enabled = false;
        _lineRenderer.gameObject.SetActive(false);
    }

    public void RoteBack(Vector2 target, BottleController newBottle)
    {
        // startPosition = transform.position;
        endPosition = originalPosition;
        bottleMask.material.DOFloat(0.7f, "_scale", _duration).SetEase(Ease.Linear);
        this.transform.DORotate(new Vector3(0, 0, 0), _duration).SetDelay(0.1f).OnComplete(() =>
        {
            this.transform.DOMove(endPosition, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _ava.sortingOrder -= 2;
                bottleMask.sortingOrder -= 2;
            });
        });
    }

    private void CaulateRoattionIndex(int value)
    {
        rotationIndex = 3 - (numberofCOlor - Mathf.Min(value, numberofTOpColorLayers));
    }

    public void StartMove(BottleController tube, bool value, float originalChildCount = 0, int index = 0)
    {
        float duration = 0.2f;
        if (value)
        {
            this.transform.DOMoveY(this.transform.position.y + 0.2f, duration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
            });
        }
        else
        {
            this.transform.DOMoveY(originalPosition.y, duration).SetEase(Ease.InQuad).OnComplete(() =>
            {
            });
        }
    }

    public bool CanSortBall(BottleController tube)
    {
        if (isEmpty())
        {
            return true;
        }
        if (_slot == datawaterColor.waterDa.Count) return false;

        if (datawaterColor.slot == datawaterColor.waterDa.Count)
        {
            return false;
        }

        bool isSameColor = GetLastWater().index == tube.GetLastWater().index;
        if (!isSameColor)
        {
            return false;
        }
        return true;
    }

    public WaterData GetLastWater()
    {
        if (!isEmpty())
        {
            WaterData data = datawaterColor.waterDa[datawaterColor.waterDa.Count - 1];
            return data;
        }
        return null;
    }

    public bool isDone()
    {
        if (Slot < datawaterColor.slot) return false;
        if (datawaterColor.waterDa.Count != datawaterColor.slot) return false;
        for (int i = datawaterColor.waterDa.Count - 1; i >= 0; i--)
        {
            if (i > 0)
            {
                if (datawaterColor.waterDa[i].index != datawaterColor.waterDa[i - 1].index)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void ChoseRotationPointAndDirection(BottleController bottle)
    {
        if (transform.position.x > bottle.transform.position.x)
        {
            choseRotationPoint = leftRotationPoint;
            //  choseRotationPoint = _linePos;
            directionMultiple = 1f;
            _posS = roteright;
        }
        else
        {
            choseRotationPoint = rightRotationPoint;
            //  choseRotationPoint = _linePos;
            directionMultiple = -1f;
            _posS = roteleft;
        }
    }

    public WaterData GetWaterData()
    {
        if (!isEmpty())
        {
            WaterData waterData = datawaterColor.waterDa[datawaterColor.waterDa.Count - 1];
            return waterData;
        }
        return null;
    }
}

public enum StateTube
{
    Active,
    Deactive,
    Moving,
}
