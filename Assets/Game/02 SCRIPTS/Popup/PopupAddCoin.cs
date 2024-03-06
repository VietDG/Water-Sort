using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAddCoin : SingletonPopup<PopupAddCoin>
{
    private int _valueCoinReward = 200;

    public void Show()
    {
        base.canCloseWithOverlay = true;
        base.Show();
    }

    public void Close()
    {
        base.Hide();
    }

    public void OnClickAddCoin()
    {
        bool _isShow = false;
        AdsManager.Instance.ShowRewardedAd(() =>
        {
            _isShow = true;
        }, () =>
        {
            if (_isShow)
            {
                PlayerData.UserData.EarnCoin(_valueCoinReward);
                Close();
            }
            else
            {
                ActionEvent.OnShowToast?.Invoke(Const.KEY_ERROR_ADS);
            }
        });
    }
}
