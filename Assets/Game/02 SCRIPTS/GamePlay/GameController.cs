using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonMonoBehaviour<GameController>
{
    [Header("-------------------------REFERENCE--------------------------------")]
    [SerializeField] GameObject _waterPrefab;
    [SerializeField] float _spaceHorizontal, _spaceVertical, _tubeHorizontalMax;
    public List<BottleController> bottleList = new List<BottleController>();
    public BottleController _holdingBottle;

    private BottleController first;
    private BottleController second;
    [Header("------------------------------------VALUE--------------------------")]
    private Camera _camera;
    [SerializeField] private float _minCameraSize;
    [SerializeField] private float _maxCameraSize;

    public override void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        InitScreen();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        int tubeNumber = 5;

        if (tubeNumber > _tubeHorizontalMax)
        {
            int top = (int)Mathf.Ceil(tubeNumber / 2f);
            int bot = (int)Mathf.Floor(tubeNumber / 2f);
            _spaceHorizontal = (_tubeHorizontalMax / top) * GetDistance(top);

            for (int i = 0; i < top; i++)
            {
                SpawnBottleWater(i, 0f, 4);
            }

            float _spaccBot = tubeNumber % 2 == 0 ? 0f : 0.5f;
            for (int i = 0; i < bot; i++)
            {
                SpawnBottleWater(i + _spaccBot, -_spaceVertical, 4);
            }
        }
        else
        {
            _spaceHorizontal = (_tubeHorizontalMax / tubeNumber) * GetDistance(tubeNumber);
            for (int i = 0; i < tubeNumber; i++)
            {
                SpawnBottleWater(i, 0f, 4);
            }
        }
    }

    public DataWaterSO _dataWaterSO;
    private void SpawnBottleWater(float x, float y, int value)
    {
        GameObject waterObj = SimplePool.Spawn(_waterPrefab, Vector2.zero, Quaternion.identity);
        BottleController bottle = waterObj.GetComponent<BottleController>();
        // bottle.Init(data, index);

        Vector2 target = new Vector2(x * (bottle.weight / 2 + _spaceHorizontal), y);

        WaterData waterData = new WaterData();

        bottle.InitPos(target, value);
        bottleList.Add(bottle);
    }

    private float GetDistance(int value)
    {
        return 0.2f;
    }

    private void InitScreen()
    {
        // Đặt tỷ lệ khung hình của camera.
        var (center, size) = CalculateOrthoSize();
        _camera.transform.position = center;
        _camera.orthographicSize = size;
    }

    private (Vector3 center, float size) CalculateOrthoSize()
    {
        var bounds = new Bounds();

        foreach (var col in FindObjectsOfType<Collider2D>())
        {
            bounds.Encapsulate(col.bounds);
        }
        bounds.Expand(1f);
        var vertical = bounds.size.y;
        var horizontal = bounds.size.x * _camera.pixelHeight / _camera.pixelWidth;
        // var size = Mathf.Clamp(Mathf.Max(horizontal, vertical) * 0.5f, _minCameraSize, _maxCameraSize);
        var size = Mathf.Max(horizontal, vertical) * 0.5f;
        var center = bounds.center + new Vector3(0, 0, -10);
        return (center, size);
    }

    public void OnClick(BottleController newBottle)
    {
        if (_holdingBottle == null)
        {
            if (newBottle.isEmpty()) return;
            _holdingBottle = newBottle;
            newBottle.StartMove(newBottle, true);
        }
        else
        {
            if (_holdingBottle.Equals(newBottle))
            {
                Debug.LogError("1");
                newBottle.StartMove(newBottle, false, 0);
                _holdingBottle = null;
            }
            else
            {
                Debug.LogError("2");
                if (!newBottle.CanSortBall(_holdingBottle))
                {
                    _holdingBottle.StartMove(_holdingBottle, false, 0);
                    newBottle.StartMove(newBottle, true);
                    _holdingBottle = newBottle;
                }
                else
                {
                    _holdingBottle.StartColorTransfer(newBottle);
                    _holdingBottle = null;
                }
            }
        }
    }
}
