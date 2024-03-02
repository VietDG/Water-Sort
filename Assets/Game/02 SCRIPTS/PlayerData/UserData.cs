using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    #region Varriables
    /// <summary>
    /// Achievements
    /// </summary>
    public int HighestLevel;
    public int BoosterBack;
    public int BoosterAddTube;
    public int Coin;
    #endregion

    #region Method
    public void UpdateHighestLevel()
    {
        if (HighestLevel < DataManager.Instance.LevelDataSO.getListLevel() - 1)
            this.HighestLevel++;
    }

    public void UpdateValueBooster(TypeBooster booster, int value)
    {
        switch (booster)
        {
            case TypeBooster.Back:
                this.BoosterBack += value;
                break;
            case TypeBooster.AddTube:
                this.BoosterAddTube += value;
                break;
        }
    }

    public void EarnCoin(int value)
    {
        this.Coin += value;
    }

    public void UseCoin(int value, Action<bool> callBack)
    {
        if (Coin < value)
        {
            callBack?.Invoke(false);
            return;
        }
        Coin -= value;
        callBack?.Invoke(true);
    }
    #endregion
}
