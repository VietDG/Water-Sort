using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TubeController;

public class GameController : SingletonMonoBehaviour<GameController>
{
    [Header("-------------------------REFERENCE--------------------------------")]
    [SerializeField] GameObject _waterPrefab;
    [SerializeField] float _spaceHorizontal, _spaceVertical, _tubeHorizontalMax;
    public List<BottleController> bottleList = new List<BottleController>();
    public BottleController _holdingBottle;

    private GameManager _gameManager;
    private UserData _userData;

    private BottleController first;
    private BottleController second;
    [Header("------------------------------------VALUE--------------------------")]
    private Camera _camera;
    [SerializeField] private float _minCameraSize;
    [SerializeField] private float _maxCameraSize;
    private List<KeyValuePair<BottleController, BottleController>> _prevTube = new List<KeyValuePair<BottleController, BottleController>>();

    public override void Awake()
    {
        _gameManager = GameManager.Instance;
        _userData = PlayerData.UserData;
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
        int tubeNumber = _gameManager.Level.tube;
        int slotTube = _gameManager.Level.tubeSlot;
        int index = 0;

        if (tubeNumber > _tubeHorizontalMax)
        {
            int top = (int)Mathf.Ceil(tubeNumber / 2f);
            int bot = (int)Mathf.Floor(tubeNumber / 2f);
            _spaceHorizontal = (_tubeHorizontalMax / top) * GetDistance(top);

            for (int i = 0; i < top; i++)
            {
                SpawnBottleWater(i, 0f, 4, GetBottleData());
            }

            float _spaccBot = tubeNumber % 2 == 0 ? 0f : 0.5f;
            for (int i = 0; i < bot; i++)
            {
                SpawnBottleWater(i + _spaccBot, -_spaceVertical, 4, GetBottleData());
            }
        }
        else
        {
            _spaceHorizontal = (_tubeHorizontalMax / tubeNumber) * GetDistance(tubeNumber);
            for (int i = 0; i < tubeNumber; i++)
            {
                SpawnBottleWater(i, 0f, 4, GetBottleData());
            }
        }
        List<WaterData> GetBottleData()
        {
            List<WaterData> dataBalls = new List<WaterData>();
            for (int j = 0; j < slotTube; j++)
            {
                if (index >= _gameManager.Level.data.Count)
                {
                    return dataBalls;
                }
                if (_gameManager.Level.data[index] < 0)
                {
                    index++;
                    continue;
                }
                WaterData data = _gameManager.getBallDataSO().GetWaterData(_gameManager.Level.data[index]);
                dataBalls.Add(data);
                index++;
            }
            return dataBalls;
        }
    }

    private void SpawnBottleWater(float x, float y, int value, List<WaterData> dataWater)
    {
        GameObject waterObj = SimplePool.Spawn(_waterPrefab, Vector2.zero, Quaternion.identity);
        BottleController bottle = waterObj.GetComponent<BottleController>();
        // bottle.Init(data, index);

        Vector2 target = new Vector2(x * (bottle.weight / 2 + _spaceHorizontal), y);

        DataBottle data = new DataBottle(value, dataWater);

        bottle.Init(data, value);

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
                newBottle.StartMove(newBottle, false, 0);
                _holdingBottle = null;
            }
            else
            {
                if (!newBottle.CanSortBall(_holdingBottle))
                {
                    _holdingBottle.StartMove(_holdingBottle, false, 0);
                    newBottle.StartMove(newBottle, true);
                    _holdingBottle = newBottle;
                }
                else
                {
                    // _holdingBottle.StartColorTransfer(newBottle);
                    SortWater(_holdingBottle, newBottle, OnMoveComplete);
                    _holdingBottle = null;
                }
            }
        }
        void OnMoveComplete()
        {
            //  newBottle.ChangeState(StateTube.Deactive);
            if (newBottle.isDone())
            {
                Debug.LogError("complete");
                //   newTube.PlayVfx();
                //  VibrationManager.Vibrate(15);
                //  SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("complete1"));
                // Debug.LogError($"{newTube.name} is done");
                //   if (ConditionWin())
                {
                    // Debug.Log("you win");
                    //    _gameManager.Win();
                    //  }
                }
            }
        }

    }

    private void SortTube(int tubeNumber)
    {
        Vector2 target;
        if (tubeNumber > _tubeHorizontalMax)
        {
            int top = (int)Mathf.Ceil(tubeNumber / 2f);
            _spaceHorizontal = (_tubeHorizontalMax / top) * GetDistance(top);
            int i, j;
            for (i = 0; i < top; i++)
            {
                target = new Vector2(i * (bottleList[i].weight + _spaceHorizontal), 0f);
                bottleList[i].SetPosition(target);
            }

            float _spaccBot = tubeNumber % 2 == 0 ? 0f : 0.5f;
            for (j = i; j < tubeNumber; j++)
            {
                target = new Vector2((j - i + _spaccBot) * (bottleList[j].weight + _spaceHorizontal), -_spaceVertical);
                bottleList[j].SetPosition(target);
            }
        }
        else
        {
            _spaceHorizontal = (_tubeHorizontalMax / tubeNumber) * GetDistance(tubeNumber);
            for (int i = 0; i < tubeNumber; i++)
            {
                target = new Vector2(i * (bottleList[i].weight + _spaceHorizontal), 0f);
                bottleList[i].SetPosition(target);
            }
        }
    }

    private void SortWater(BottleController from, BottleController to, Action callBack)
    {
        WaterData holdingBall = from.GetWaterData();
        List<WaterData> moveBalls = new List<WaterData>();
        List<WaterData> holdBalls = new List<WaterData>(from.datawaterColor.waterDa);
        int max = 0;
        // float t = 0;

        for (int i = holdBalls.Count - 1; i >= 0; i--)
        {
            if (!to.isEmpty() && holdingBall.index != to.GetWaterData().index)
            {
                moveBalls.Add(to.datawaterColor.waterDa[i]);

                from.datawaterColor.waterDa.RemoveAt(i);

                for (i = holdBalls.Count - 1; i >= 0; i--)
                {
                    if (holdBalls[i].index != holdingBall.index) break;

                    // Do some thing
                    max++;
                }
                break;
            }
            if (holdingBall.index == holdBalls[i].index)
            {
                if (to.datawaterColor.waterDa.Count + moveBalls.Count >= to.Slot)
                {
                    for (int j = holdBalls.Count - 1 - max; j >= 0; j--)
                    {
                        if (holdBalls[j].index != holdingBall.index) break;
                        //Do some thing
                        // t += 0.1f;
                    }
                    //Do some thing
                    break;
                }
                moveBalls.Add(from.datawaterColor.waterDa[i]);

                from.datawaterColor.waterDa.RemoveAt(i);

                max++;
            }
            else break;
        }

        int countBall = to.datawaterColor.waterDa.Count;

        for (int i = 0; i < moveBalls.Count; i++)
        {
            if (i == moveBalls.Count - 1)
            {
                from.StartMove(from, to, countBall, i);   //move -> new branch
                _holdingBottle.StartColorTransfer(to);
            }
            else
            {
                from.StartMove(from, to, countBall, i);   //move -> new branch
                _holdingBottle.StartColorTransfer(to);
            }

            to.datawaterColor.waterDa.Add(moveBalls[i]);
            AddPrevTube(from, to);
        }
    }

    private void AddPrevTube(BottleController from, BottleController to)
    {
        _prevTube.Add(new KeyValuePair<BottleController, BottleController>(from, to));
    }

}
