using SS.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("~~~DATA~~~")]
    public DataWaterSO _dataWaterSO;
    public GameController controller;
    public TopDataSO topDataSO;
    public BgDataSO bgDataSO;
    [field: SerializeField] public Level Level { get; private set; }
    public DataManager Datamanager { get; private set; }
    private UserData _userData;
    [Header("REFFERENCE")]
    [SerializeField] int _level;
    [SerializeField] TutorialController _tutorialController;
    public TutorialController TutorialController => _tutorialController;

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
    }

    public DataWaterSO getBallDataSO()
    {
        return _dataWaterSO;
    }

    public Sprite getTop()
    {
        int id = CollectionData.ShopData.getItemEquip(TypeSkinCollection.Ball).Index;
        return topDataSO.getTop(id);
    }

    public Sprite getBG()
    {
        int id = CollectionData.ShopData.getItemEquip(TypeSkinCollection.Theme).Index;
        return bgDataSO.getBG(id);
    }

    public void Win()
    {
        _userData.UpdateHighestLevel();
        //FunctionCommon.DelayTime(5, () =>
        //{
        //});
        ActionEvent.OnNextLevel?.Invoke();
        FunctionCommon.DelayTime(1.5f, () =>
        {
            SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("victory1"));
            // ActionEvent.OnResetGamePlay?.Invoke();
            PopupWin.Instance.Show();
        });
    }

    public void OnClickRestart()
    {
        if (!controller.isMoving()) return;
        ActionEvent.OnResetGamePlay?.Invoke();
    }
}
