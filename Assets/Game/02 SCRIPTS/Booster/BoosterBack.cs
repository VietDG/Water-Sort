using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterBack : BoosterController
{
    private void Awake()
    {
        ActionEvent.OnResetGamePlay += UpdateBooster;
    }
    void Start()
    {
        UpdateBooster();
    }

    private void OnDestroy()
    {
        ActionEvent.OnResetGamePlay -= UpdateBooster;
    }

    public void OnClickBack()
    {
        if (!GameManager.Instance._controller.isMoving()) return;
        if (PlayerData.UserData.BoosterBack > 0)
        {
            ActionEvent.OnUserBoosterBack?.Invoke();
            DisPlayBooster(PlayerData.UserData.BoosterBack);
        }
        else
        {
            Debug.LogError("Show Ads");
        }
    }

    private void UpdateBooster()
    {
        int amout = PlayerData.UserData.BoosterBack;
        PlayerData.UserData.UpdateValueBooster(this.Type, 5 - amout);
        base.DisPlayBooster(PlayerData.UserData.BoosterBack);
    }
}
