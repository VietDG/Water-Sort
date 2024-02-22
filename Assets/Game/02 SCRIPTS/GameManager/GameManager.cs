using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("~~~DATA~~~")]
    public DataWaterSO _dataWaterSO;
    [field: SerializeField] public Level Level { get; private set; }
    public DataManager Datamanager { get; private set; }
    private UserData _userData;
    [Header("REFFERENCE")]
    [SerializeField] int _level;

    public override void Awake()
    {
        Datamanager = DataManager.Instance;
        _userData = PlayerData.UserData;

        InitLevel();
    }

    public void InitLevel()
    {
        this.Level = Datamanager.LevelDataSO.getLevel(_userData.HighestLevel);
    }

    public DataWaterSO getBallDataSO()
    {
        return _dataWaterSO;
    }
}
