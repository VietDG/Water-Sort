using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class DataWaterColor
{
    public int slot;
    public List<Color> color = new List<Color>();

    public DataWaterColor(int slot, List<Color> color)
    {
        this.slot = slot;
        this.color = color;
    }
    public DataWaterColor() { }
}

public class BottleController : MonoBehaviour
{
    public List<Color> bottleColors;
    public SpriteRenderer bottleMask;
    public SpriteRenderer _ava;
    public WaterData data;
    public DataWaterColor datawaterColor;
    public int Id => data.index;

    private int _slot;
    public int Slot => _slot;

    public LineRenderer _lineRenderer;

    public AnimationCurve scaleAndRotete;
    public AnimationCurve fillAmoutCurve;
    public AnimationCurve rotationSpeed;
    public float timeRotate = 1f;
    //------------------------------//
    /// <summary>
    /// 
    /// </summary>
    public float[] fillAmouts;
    public float[] rotationValues;
    private int rotationIndex = 0;
    [Range(0, 4)]
    public int numberofCOlor = 4;
    private Color topColor;
    public int numberofTOpColorLayers = 1;

    public BottleController bottleCOntrollerRef;
    private int numberOfColorToTransfer = 0;

    public Transform leftRotationPoint;
    public Transform rightRotationPoint;
    private Transform choseRotationPoint;
    [SerializeField] Transform _spawnTrans;
    [SerializeField] Transform _startTeransMove;

    private float directionMultiple = 1f;

    Vector3 originalPosition;
    Vector3 startPosition;
    Vector3 endPosition;


    public float weight;
    public float height;

    private Vector2 _startPosMove;
    public Vector2 StartPosMove => _startPosMove;

    private void Awake()
    {
        weight = _ava.bounds.size.x;
        height = _ava.bounds.size.y;
    }

    private void OnMouseDown()
    {
        GameController.Instance.OnClick(this);
    }

    void Start()
    {
        HandleColor();

        originalPosition = transform.position;

        UpdateColorOnShader();
        UpdateTopColor();
    }

    public bool isEmpty()
    {
        if (numberofCOlor > 0)
        {
            return false;
        }
        return true;
    }

    public void HandleColor()
    {
        bottleMask.material.SetFloat("_FillAmout", fillAmouts[numberofCOlor]);
    }

    public void Init(WaterData data, DataWaterColor dataColor)
    {
        this.data = data;
        bottleColors = dataColor.color;
        this._slot = dataColor.slot;
        numberofCOlor = dataColor.slot;
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
        numberOfColorToTransfer = Mathf.Min(numberofTOpColorLayers, 4 - bottle.numberofCOlor);

        for (int i = 0; i < numberOfColorToTransfer; i++)
        {
            bottle.bottleColors[bottle.numberofCOlor + i] = topColor;
        }

        bottle.UpdateColorOnShader();
        UpdateColorOnShader();
        CaulateRoattionIndex(4 - bottle.numberofCOlor);

        transform.GetComponent<SpriteRenderer>().sortingOrder += 2;
        bottleMask.sortingOrder += 2;
        StartCoroutine(MoveBottle(bottle));
    }

    IEnumerator MoveBottle(BottleController bottle)
    {
        startPosition = transform.position;

        if (choseRotationPoint == leftRotationPoint)
        {
            endPosition = bottle.rightRotationPoint.position;
        }
        else
        {
            endPosition = bottle.leftRotationPoint.position;
        }

        float t = 0;

        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            t += Time.deltaTime * 2;

            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;

        StartCoroutine(RotateBottle(bottle));
    }

    IEnumerator MoveBottleBack()
    {
        startPosition = transform.position;

        endPosition = originalPosition;

        float t = 0;

        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            t += Time.deltaTime * 2;

            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;

        transform.GetComponent<SpriteRenderer>().sortingOrder -= 2;
        bottleMask.sortingOrder -= 2;
    }

    void UpdateColorOnShader()
    {
        bottleMask.material.SetColor($"{ColorShader.C1}", bottleColors[0]);
        bottleMask.material.SetColor($"{ColorShader.C2}", bottleColors[1]);
        bottleMask.material.SetColor($"{ColorShader.C3}", bottleColors[2]);
        bottleMask.material.SetColor($"{ColorShader.C4}", bottleColors[3]);
    }

    IEnumerator RotateBottle(BottleController bottle)
    {
        float t = 0;
        float lerpValue;
        float angleValue;

        float lastAngleValue = 0;

        while (t < timeRotate)
        {
            lerpValue = t / timeRotate;
            angleValue = Mathf.Lerp(0.0f, directionMultiple * rotationValues[rotationIndex], lerpValue);

            // transform.eulerAngles = new Vector3(0, 0, angleValue);
            transform.RotateAround(choseRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);

            bottleMask.material.SetFloat("_scale", scaleAndRotete.Evaluate(angleValue));

            if (fillAmouts[numberofCOlor] > fillAmoutCurve.Evaluate(angleValue) + 0.005f)
            {
                if (_lineRenderer.enabled == false)
                {
                    Debug.LogError("c");
                    _lineRenderer.startColor = topColor;
                    _lineRenderer.endColor = topColor;

                    _lineRenderer.SetPosition(0, choseRotationPoint.position);
                    _lineRenderer.SetPosition(1, choseRotationPoint.position - Vector3.up * 1.45f);
                    _lineRenderer.enabled = true;
                }
                bottleMask.material.SetFloat("_FillAmout", fillAmoutCurve.Evaluate(angleValue));
                bottle.FillUp(fillAmoutCurve.Evaluate(lastAngleValue) - fillAmoutCurve.Evaluate(angleValue));
            }

            t += Time.deltaTime * rotationSpeed.Evaluate(angleValue);
            lastAngleValue = angleValue;
            yield return new WaitForEndOfFrame();
        }
        angleValue = directionMultiple * rotationValues[rotationIndex];
        // transform.eulerAngles = new Vector3(0, 0, angleValue);
        bottleMask.material.SetFloat("_scale", scaleAndRotete.Evaluate(angleValue));
        bottleMask.material.SetFloat("_FillAmout", fillAmoutCurve.Evaluate(angleValue));

        numberofCOlor -= numberOfColorToTransfer;
        bottle.numberofCOlor += numberOfColorToTransfer;

        _lineRenderer.enabled = false;
        StartCoroutine(RotateBottleBack());
    }

    IEnumerator RotateBottleBack()
    {
        float t = 0;
        float lerpValue;
        float angleValue;

        float lastAngleValue = directionMultiple * rotationValues[rotationIndex];

        while (t < timeRotate)
        {
            lerpValue = t / timeRotate;
            angleValue = Mathf.Lerp(directionMultiple * rotationValues[rotationIndex], 0f, lerpValue);

            //  transform.eulerAngles = new Vector3(0, 0, angleValue);
            transform.RotateAround(choseRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);

            bottleMask.material.SetFloat("_scale", scaleAndRotete.Evaluate(angleValue));

            lastAngleValue = angleValue;
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        UpdateTopColor();
        angleValue = 0f;
        transform.eulerAngles = new Vector3(0, 0, angleValue);
        bottleMask.material.SetFloat("_scale", scaleAndRotete.Evaluate(angleValue));

        StartCoroutine(MoveBottleBack());
    }

    public void UpdateTopColor() // update mau
    {
        if (numberofCOlor != 0)
        {
            numberofTOpColorLayers = 1;
            topColor = bottleColors[numberofCOlor - 1];

            if (numberofCOlor == 4)
            {
                if (bottleColors[3].Equals(bottleColors[2]))
                {
                    numberofTOpColorLayers = 2;
                    if (bottleColors[2].Equals(bottleColors[1]))
                    {
                        numberofTOpColorLayers = 3;
                        if (bottleColors[1].Equals(bottleColors[0]))
                        {
                            numberofTOpColorLayers = 4;
                        }
                    }
                }
            }
            else if (numberofCOlor == 3)
            {
                if (bottleColors[2].Equals(bottleColors[1]))
                {
                    numberofTOpColorLayers = 2;
                    if (bottleColors[1].Equals(bottleColors[0]))
                    {
                        numberofTOpColorLayers = 3;
                    }
                }
            }
            else if (numberofCOlor == 2)
            {
                if (bottleColors[1].Equals(bottleColors[0]))
                {
                    numberofTOpColorLayers = 2;
                }
            }
            rotationIndex = 3 - (numberofCOlor - numberofTOpColorLayers);
        }
    }

    public bool FillBOttleCheck(Color colorToCheck)
    {
        if (numberofCOlor == 0)
        {
            return true;
        }
        else
        {
            if (numberofCOlor == 4)
            {
                return false;
            }
            else
            {
                if (topColor.Equals(colorToCheck))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    private void CaulateRoattionIndex(int value)
    {
        rotationIndex = 3 - (numberofCOlor - Mathf.Min(value, numberofTOpColorLayers));
    }

    private void FillUp(float value)// hoi day 
    {
        bottleMask.material.SetFloat("_FillAmout", bottleMask.material.GetFloat("_FillAmout") + value);
    }


    public void StartMove(BottleController tube, bool value, float originalChildCount = 0, int index = 0)
    {
        float duration = 0.5f;
        if (value)
        {
            this.transform.DOMoveY(tube.StartPosMove.y, duration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
            });
        }
        else
        {
            this.transform.DOMoveY(0f, duration).SetEase(Ease.InQuad).OnComplete(() =>
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
        if (_slot == numberofCOlor) return false;

        if (4 == numberofCOlor)
        {
            return false;
        }

        bool isSameColor = Id == tube.Id;
        if (!isSameColor)
        {
            return false;
        }
        return true;
    }

    private void ChoseRotationPointAndDirection(BottleController bottle)
    {
        if (transform.position.x > bottle.transform.position.x)
        {
            choseRotationPoint = leftRotationPoint;
            directionMultiple = -1f;
        }
        else
        {
            choseRotationPoint = rightRotationPoint;
            directionMultiple = 1f;
        }
    }
}
