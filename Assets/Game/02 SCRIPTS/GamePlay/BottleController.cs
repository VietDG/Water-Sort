using DG.Tweening;
using PopupSystem;
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
    // public List<Color> bottleColors;
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

    public GameObject _bottleTop;
    public Transform leftRotationPoint;
    public Transform rightRotationPoint;
    private Transform choseRotationPoint;
    [SerializeField] Transform _spawnTrans;
    [SerializeField] Transform _startTeransMove;
    public Transform _linePos;

    public Transform _posS;

    public Transform roteleft, roteright;

    private float directionMultiple = 1f;

    public Vector3 originalPosition;
    Vector3 endPosition;

    public float weight;
    public float height;

    private Vector2 _startPosMove;
    public Vector2 StartPosMove => _startPosMove;

    private Vector3 _leftPos;
    public Vector3 LeftPos => _leftPos;

    private Vector3 _rightPos;
    public Vector3 RightPos => _rightPos;

    [SerializeField] ParticleSystem _vfx;
    [field: SerializeField] public StateTube state { get; private set; }
    public string[] _colorName = new string[] { "C1", "C2", "C3", "C4" }; // Update Start Color
    private float _duration = 0.5f;

    #region UNITY_METHOD
    private void Awake()
    {
        weight = _ava.bounds.size.x;
        height = _ava.bounds.size.y;
    }

    private void OnMouseDown()
    {
        if (PopupManager.Instance.hasPopupShowing) return;
        if (!IsTutLevel()) return;
        if (state.Equals(StateTube.Active) || state.Equals(StateTube.Moving)) return;
        GameManager.Instance.controller.OnClick(this);
    }
    private void OnEnable()
    {
        ActionEvent.OnResetGamePlay += Reset;
        ActionEvent.OnSetSkin += SetSkinTop;
    }

    private void Reset()
    {
        this.state = StateTube.Deactive;
        _vfx.Stop();
        _bottleTop.SetActive(false);
    }

    public void StopVfx()
    {
        _vfx.Stop();
    }

    private void OnDisable()
    {
        Reset();
        ActionEvent.OnResetGamePlay -= Reset;
        ActionEvent.OnSetSkin -= SetSkinTop;
    }
    #endregion


    #region Skin
    private void SetSkinTop(TypeSkinCollection type)
    {
        if (type.Equals(TypeSkinCollection.Ball))
        {
            Sprite newAva = GameManager.Instance.getTop();
            _bottleTop.GetComponent<SpriteRenderer>().sprite = newAva;
        }
    }

    #endregion

    #region HandleStart
    public void HandleColor()
    {
        bottleMask.material.SetFloat("_FillAmout", fillAmouts[datawaterColor.waterDa.Count]);
    }
    public void Init(DataBottle dataColor, int value)
    {
        datawaterColor = dataColor;
        dataColor.slot = value;
        this._slot = dataColor.slot;
        HandleColor();
        SetSkinTop(TypeSkinCollection.Ball);
        _bottleTop.SetActive(false);
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
        originalPosition = target;
    }

    public void UpdateStartColor()
    {
        for (int i = 0; i < datawaterColor.waterDa.Count; i++)
        {
            bottleMask.material.SetColor(_colorName[i], datawaterColor.waterDa[i].color);
        }
    }
    #endregion

    #region MoveMent

    public void StartColorTransfer(BottleController bottle)
    {
        ChoseRotationPointAndDirection(bottle);
        _ava.sortingOrder += 3;
        bottleMask.sortingOrder += 3;
        Rote(bottle);
        this.ChangeState(StateTube.Moving);
        bottle.ChangeState(StateTube.Open);
    }

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

        this.transform.DOMove(target, 0.2f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            if (datawaterColor.waterDa.Count == 0)
            {
                bottleMask.material.DOFloat(0.07f, "_scale", _duration).SetEase(Ease.Linear);
            }
            if (datawaterColor.waterDa.Count == 1)
            {
                bottleMask.material.DOFloat(0.17f, "_scale", _duration).SetEase(Ease.Linear);
            }
            if (datawaterColor.waterDa.Count == 2)
            {
                bottleMask.material.DOFloat(0.35f, "_scale", _duration).SetEase(Ease.Linear);
            }
            if (datawaterColor.waterDa.Count == 3)
            {
                bottleMask.material.DOFloat(0.3f, "_scale", _duration).SetEase(Ease.Linear);
            }

            SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("Water"));
            bottleMask.material.DOFloat(fillAmouts[datawaterColor.waterDa.Count], "_FillAmout", _duration).SetEase(Ease.Linear);

            bottle.bottleMask.material.DOFloat(bottle.fillAmouts[bottle.datawaterColor.waterDa.Count], "_FillAmout", _duration).SetEase(Ease.Linear).SetDelay(0.2f);
            bottle.UpdateStartColor();

            FunctionCommon.DelayTime(0.2f, () =>
            {
                StartCoroutine(SetLine(bottle));
            });
            this.transform.DORotate(new Vector3(0, 0, directionMultiple * rotationValues[datawaterColor.waterDa.Count]), _duration).OnComplete(() =>
            {
                RoteBack(target, bottle);
            });
        });
    }

    public void SetColorBooster()
    {
        if (datawaterColor.waterDa.Count == 0)
        {
            bottleMask.material.SetFloat("_FillAmout", fillAmouts[0]);
            return;
        }
        bottleMask.material.SetFloat("_FillAmout", fillAmouts[datawaterColor.waterDa.Count]);
        // AddColor();
        for (int i = 0; i < datawaterColor.waterDa.Count; i++)
        {
            bottleMask.material.SetColor(_colorName[i], datawaterColor.waterDa[i].color);
        }
        _bottleTop.SetActive(false);
    }

    IEnumerator SetLine(BottleController bottle)
    {
        float t = 0;
        _lineRenderer.gameObject.SetActive(true);
        _lineRenderer.startColor = bottle.GetWaterData().color;//
        _lineRenderer.endColor = bottle.GetWaterData().color;
        _lineRenderer.enabled = true;
        ChoseRotationPointAndDirection(bottle);
        while (t <= 0.4f)
        {
            _lineRenderer.SetPosition(0, choseRotationPoint.position);
            _lineRenderer.SetPosition(1, new Vector3(choseRotationPoint.position.x, bottle._linePos.position.y, 0) /*- Vector3.up * 0.6f*/);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _lineRenderer.enabled = false;
        _lineRenderer.gameObject.SetActive(false);
    }

    public void RoteBack(Vector2 target, BottleController newBottle)
    {
        endPosition = originalPosition;
        bottleMask.material.DOFloat(0.7f, "_scale", _duration - 0.1f).SetEase(Ease.Linear);
        this.transform.DORotate(new Vector3(0, 0, 0), _duration).OnComplete(() =>
        {
            this.transform.DOMove(endPosition, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                _ava.sortingOrder -= 3;
                bottleMask.sortingOrder -= 3;
                this.ChangeState(StateTube.Deactive);
                newBottle.ChangeState(StateTube.Deactive);
            });
        });
    }

    public void StartMove(BottleController tube, bool value)
    {
        float duration = 0.2f;
        if (value)
        {
            this.transform.DOMoveY(this.transform.position.y + 0.5f, duration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                tube.ChangeState(StateTube.Deactive);
            });
        }
        else
        {
            this.transform.DOMoveY(originalPosition.y, duration).SetEase(Ease.InQuad).OnComplete(() =>
            {
                tube.ChangeState(StateTube.Deactive);
            });
        }
    }

    #endregion

    public void Complete()
    {
        _bottleTop.gameObject.SetActive(true);
        SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("TopComplete"));
        _vfx.Play();
    }

    public bool isEmpty()
    {
        if (datawaterColor.waterDa.Count > 0)
        {
            return false;
        }
        return true;
    }

    public void ChangeState(StateTube state)
    {
        this.state = state;
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

        bool isSameColor = GetWaterData().index == tube.GetWaterData().index;
        if (!isSameColor)
        {
            return false;
        }
        return true;
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
            directionMultiple = 1f;
            _posS = roteright;
        }
        else
        {
            choseRotationPoint = rightRotationPoint;
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

    private bool IsTutLevel()
    {
        if (GameManager.Instance.Level.level == 1)
        {
            if (datawaterColor.waterDa.Count == 3 && !GameManager.Instance.TutorialController.IsTutLevel1)
            {
                return false;
            }
            if (datawaterColor.waterDa.Count == 1 && GameManager.Instance.TutorialController.IsTutLevel1)
            {
                return false;
            }
            if (!GameManager.Instance.TutorialController.IsTutLevel1) GameManager.Instance.TutorialController.DisplayTutLevel1Step2();
        }
        return true;
    }
}

public enum StateTube
{
    Active,
    Deactive,
    Moving,
    Open,
}
