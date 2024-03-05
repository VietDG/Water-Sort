using SS.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("~~~DATA~~~")]
    public DataWaterSO _dataWaterSO;
    public GameController _controller;
    [field: SerializeField] public Level Level { get; private set; }
    public DataManager Datamanager { get; private set; }
    private UserData _userData;
    [Header("REFFERENCE")]
    [SerializeField] int _level;

    public override void Awake()
    {
        Datamanager = DataManager.Instance;
        _userData = PlayerData.UserData;
        ActionEvent.OnNextLevel += InitLevel;
        InitLevel();
    }

    private void OnDestroy()
    {
        ActionEvent.OnNextLevel -= InitLevel;
    }

    public void InitLevel()
    {
        this.Level = Datamanager.LevelDataSO.getLevel(_userData.HighestLevel);
        Debug.LogError(_userData.HighestLevel);
    }

    public DataWaterSO getBallDataSO()
    {
        return _dataWaterSO;
    }

    public void Win()
    {
        _userData.UpdateHighestLevel();
        //FunctionCommon.DelayTime(5, () =>
        //{
        //});
        ActionEvent.OnNextLevel?.Invoke();
        FunctionCommon.DelayTime(2f, () =>
        {
            ActionEvent.OnResetGamePlay?.Invoke();
        });
    }

    public void OnClickRestart()
    {
        if (_controller.isMoving()) return;
        ActionEvent.OnResetGamePlay?.Invoke();
    }
}
