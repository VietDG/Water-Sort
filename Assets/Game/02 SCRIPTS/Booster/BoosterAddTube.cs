using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterAddTube : BoosterController
{
    private void Awake()
    {
        ActionEvent.OnResetGamePlay += UpdateQuality;
    }

    public void Start()
    {
        UpdateQuality();
    }

    private void OnDestroy()
    {
        ActionEvent.OnResetGamePlay -= UpdateQuality;
    }

    public void OnClickAddTube()
    {
        if (!GameManager.Instance.controller.isMoving()) return;
        if (PlayerData.UserData.BoosterAddTube > 0)
        {
            ActionEvent.OnUserBoosterAddTube?.Invoke();
            DisPlayBooster(PlayerData.UserData.BoosterAddTube);
        }
        else
        {
            bool _isShow = false;
            AdsManager.Instance.ShowRewardedAd(() =>
            {
                _isShow = true;
            }, () =>
            {
                if (_isShow)
                {
                    PlayerData.UserData.BoosterAddTube.RefIntCrementWithAmount(5);
                    UpdateQuality();
                }
                else
                {
                    ActionEvent.OnShowToast?.Invoke(Const.KEY_ERROR_ADS);
                }
            });
        }
    }

    private void UpdateQuality()
    {
        int quatily = PlayerData.UserData.BoosterAddTube;
        PlayerData.UserData.UpdateValueBooster(this.Type, 1 - quatily);
        base.DisPlayBooster(PlayerData.UserData.BoosterAddTube);
    }

}
