using SS.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        ActionEvent.OnUserBoosterBack += UserBack;
        ActionEvent.OnResetGamePlay += Reset;
        ActionEvent.OnUserBoosterAddTube += UseAddTube;
        //   Init();
    }

    private void OnDestroy()
    {
        ActionEvent.OnUserBoosterBack -= UserBack;
        ActionEvent.OnResetGamePlay -= Reset;
        ActionEvent.OnUserBoosterAddTube -= UseAddTube;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(Const.SCENE_GAME);
        }
        //    InitScreen();
#endif
    }

    private void Start()
    {
        Init();
        Invoke(nameof(InitScreen), 0.02f);
        //InitScreen();
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
                SpawnBottleWater(i, 0f, slotTube, GetBottleData());
            }

            float _spaccBot = tubeNumber % 2 == 0 ? 0f : 0.5f;
            for (int i = 0; i < bot; i++)
            {
                SpawnBottleWater(i + _spaccBot, -_spaceVertical, slotTube, GetBottleData());
            }
        }
        else
        {
            _spaceHorizontal = (_tubeHorizontalMax / tubeNumber) * GetDistance(tubeNumber);
            for (int i = 0; i < tubeNumber; i++)
            {
                SpawnBottleWater(i, 0f, slotTube, GetBottleData());
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

    public void Reset()
    {
        foreach (var item in bottleList)
        {
            item.bottleColors.Clear();
            SimplePool.Despawn(item.gameObject);
        }
        bottleList.Clear();
        _holdingBottle = null;
        Init();
        Invoke(nameof(InitScreen), 0.02f);
        // InitScreen();
    }

    private void SpawnBottleWater(float x, float y, int value, List<WaterData> dataWater)
    {
        GameObject waterObj = SimplePool.Spawn(_waterPrefab, Vector2.zero, Quaternion.identity);
        BottleController bottle = waterObj.GetComponent<BottleController>();
        // bottle.Init(data, index);

        Vector2 target = new Vector2(x * (bottle.weight / 2 + _spaceHorizontal), y);

        DataBottle data = new DataBottle(value, dataWater);

        bottle.InitPos(target, value);
        bottle.Init(data, value);

        bottleList.Add(bottle);
    }

    private float GetDistance(int value)
    {
        if (value <= 2)
        {
            Debug.LogError("2");
            return 0.3f;
        }
        else if (value > 2 && value <= 5)
        {
            Debug.LogError("3");
            return 0.4f;
        }
        else if (value > 5 && value <= 7)
        {
            Debug.LogError("5");
            return 0.6f;
        }
        Debug.LogError("none");
        return 0.4f;
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

        foreach (var col in FindObjectsOfType<BoxCollider2D>())
        {
            bounds.Encapsulate(col.bounds);
        }
        //   bounds.Expand(3f);
        var vertical = bounds.size.y;
        var horizontal = bounds.size.x * _camera.pixelHeight / _camera.pixelWidth;
        var size = Mathf.Clamp(Mathf.Max(horizontal, vertical) * 0.5f, _minCameraSize, _maxCameraSize);
        //  var size = Mathf.Max(horizontal, vertical) * 0.4f;
        var center = bounds.center + new Vector3(0, 0, -10);
        return (center, size);
    }

    public void OnClick(BottleController newBottle)
    {
        if (_holdingBottle == null)
        {
            if (newBottle.isEmpty()) return;
            if (newBottle.isDone()) return;
            newBottle.ChangeState(StateTube.Active);
            _holdingBottle = newBottle;
            newBottle.StartMove(newBottle, true);
        }
        else
        {
            if (_holdingBottle.Equals(newBottle))
            {
                newBottle.ChangeState(StateTube.Active);
                newBottle.StartMove(newBottle, false, 0);
                _holdingBottle = null;
            }
            else
            {
                if (!newBottle.CanSortBall(_holdingBottle))
                {
                    _holdingBottle.ChangeState(StateTube.Deactive);
                    newBottle.ChangeState(StateTube.Active);
                    _holdingBottle.StartMove(_holdingBottle, false, 0);
                    newBottle.StartMove(newBottle, true);
                    _holdingBottle = newBottle;
                }
                else
                {
                    newBottle.ChangeState(StateTube.Moving);
                    // _holdingBottle.StartColorTransfer(newBottle);
                    SortWater(_holdingBottle, newBottle, OnMoveComplete);
                    _holdingBottle = null;
                }
            }
        }
        void OnMoveComplete()
        {
            //  newBottle.ChangeState(StateTube.Deactive);
            // newBottle.ChangeState(StateTube.Deactive);
            if (newBottle.isDone())
            {
                FunctionCommon.DelayTime(1f, () =>
                {
                    newBottle.Complete();
                });
                //   newTube.PlayVfx();
                //  VibrationManager.Vibrate(15);
                //  SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("complete1"));
                // Debug.LogError($"{newTube.name} is done");
                if (ConditionWin())
                {
                    // Debug.Log("you win");
                    //    _gameManager.Win();
                    //  }
                    _gameManager.Win();
                }
            }
        }

    }

    private bool ConditionWin()
    {
        for (int i = 0; i < bottleList.Count; i++)
        {
            if (bottleList[i].isEmpty()) continue;
            if (!bottleList[i].isDone()) return false;
        }
        return true;
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

                to.bottleColors.Add(from.datawaterColor.waterDa[i].color);
                from.datawaterColor.waterDa.RemoveAt(i);
                from.bottleColors.RemoveAt(i);
                //from.UpdateStartColor();
                //to.UpdateStartColor();

                max++;
            }
            else break;
        }

        int countBall = to.datawaterColor.waterDa.Count;

        for (int i = 0; i < moveBalls.Count; i++)
        {
            if (i == moveBalls.Count - 1)
            {
                //  from.StartMove(from, to, countBall, i);   //move -> new branch
                _holdingBottle.StartColorTransfer(to);
            }
            else
            {
                //    from.StartMove(from, to, countBall, i);   //move -> new branch
                _holdingBottle.StartColorTransfer(to);
            }

            to.datawaterColor.waterDa.Add(moveBalls[i]);
            AddPrevTube(from, to);
            callBack?.Invoke();
        }
    }

    private void AddPrevTube(BottleController from, BottleController to)
    {
        _prevTube.Add(new KeyValuePair<BottleController, BottleController>(from, to));
    }

    #region Booster

    private void UserBack()
    {
        if (_prevTube.Count > 0)
        {
            BottleController from = _prevTube[_prevTube.Count - 1].Key;
            BottleController to = _prevTube[_prevTube.Count - 1].Value;
            if (_holdingBottle != null)
            {
                if (to != _holdingBottle)
                    _holdingBottle.StartMove(_holdingBottle, false, _holdingBottle.datawaterColor.waterDa.Count - 1);
                _holdingBottle = null;
            }

            int countBall = from.datawaterColor.waterDa.Count;
            List<WaterData> moveBalls = new List<WaterData>();
            int cd = 1;
            for (int i = _prevTube.Count - 1; i >= 0; i--)
            {
                BottleController first = _prevTube[i].Value;
                BottleController second = _prevTube[i].Key;
                moveBalls.Add(first.GetLastWater());
                second.datawaterColor.waterDa.Add(first.GetLastWater());
                first.datawaterColor.waterDa.Remove(first.GetLastWater());
                cd++;
                if (_prevTube.Count == 1 || first.datawaterColor.waterDa.Count <= 0) break;
                if (first != _prevTube[i - 1].Value || second != _prevTube[i - 1].Key)
                {
                    break;
                }
            }

            from.ChangeState(StateTube.Active);
            to.ChangeState(StateTube.Moving);
            for (int i = 0; i < moveBalls.Count; i++)
            {
                if (i == moveBalls.Count - 1)
                {
                    // _holdingBottle.StartColorTransfer(to);
                    //moveBalls[i].Start(to, from, countBall, i, () =>
                    //{
                    from.ChangeState(StateTube.Deactive);
                    to.ChangeState(StateTube.Deactive);
                    //});
                    // moveBalls[i].Set
                    from.SetColorBooster();
                    to.SetColorBooster();
                }
                else
                {
                    from.SetColorBooster();
                    to.SetColorBooster();
                    //   moveBalls[i].Movement(to, from, countBall, i, null);
                    //_holdingBottle.StartColorTransfer(to);
                }

                if (_prevTube.Count > 0)
                {
                    _prevTube.RemoveAt(_prevTube.Count - 1);
                }
            }

            PlayerData.UserData.UpdateValueBooster(TypeBooster.Back, -1);
        }
        else
        {
            Debug.Log("Khong co luot de su dung booster");
        }
    }

    private void UseAddTube()
    {
        int slotTube = _gameManager.Level.tubeSlot;

        if (bottleList.Count <= _gameManager.Level.tube)
        {
            SpawnBottleWater(0f, 0f, 4, new List<WaterData>());
            SortTube(bottleList.Count);
            Invoke(nameof(InitScreen), 0.02f);
        }
        else
        {
            Debug.LogError("da du canh");
        }
        PlayerData.UserData.UpdateValueBooster(TypeBooster.Tube, -1);
    }

    public void OnClickRestart()
    {
        if (!isMoving()) return;
        ActionEvent.OnResetGamePlay?.Invoke();
    }

    public bool isMoving()
    {
        foreach (var item in GameController.Instance.bottleList)
        {
            if (item.state.Equals(StateTube.Moving))
            {
                return false;
            }
        }
        return true;
    }
    #endregion
}
