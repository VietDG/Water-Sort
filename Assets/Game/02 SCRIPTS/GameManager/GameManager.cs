using SS.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            AdsManager.Instance.ShowInterstitial();
        }
    }

    public void InitLevel()
    {
        AdsManager.Instance.ShowBanner();
        this.Level = Datamanager.LevelDataSO.getLevel(_userData.HighestLevel);
        GlobalEventManager.OnLevelPlay(Level.level);
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
        ActionEvent.OnNextLevel?.Invoke();
        Datamanager.SaveAllData();
        foreach (var item in controller.bottleList)
        {
            item.StopVfx();
        }
        GlobalEventManager.OnLevelComplete(Level.level);
        FunctionCommon.DelayTime(2f, () =>
        {
            AdsManager.Instance.ShowInterstitial(() =>
            {
                SoundManager.Instance.PlaySfxNoRewind(GlobalSetting.GetSFX("victory1"));
                PopupWin.Instance.Show();
            });
        });
    }

    public void OnClickRestart()
    {
        if (!controller.isMoving()) return;
        AdsManager.Instance.ShowInterstitial(() =>
        {

            ActionEvent.OnResetGamePlay?.Invoke();
            GlobalEventManager.OnLevelReplay(Level.level);
        });
    }
}
