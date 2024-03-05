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
        if (!GameManager.Instance._controller.isMoving()) return;
        if (PlayerData.UserData.BoosterAddTube > 0)
        {
            ActionEvent.OnUserBoosterAddTube?.Invoke();
            DisPlayBooster(PlayerData.UserData.BoosterAddTube);
        }
        else
        {
            Debug.LogError("Show Ads");
        }
    }

    private void UpdateQuality()
    {
        int quatily = PlayerData.UserData.BoosterAddTube;
        PlayerData.UserData.UpdateValueBooster(this.Type, 1 - quatily);
        base.DisPlayBooster(PlayerData.UserData.BoosterAddTube);
    }

}
