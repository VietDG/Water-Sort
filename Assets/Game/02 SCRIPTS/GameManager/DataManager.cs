using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    public LevelDataSO LevelDataSO;

    #region Unity Method
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("Is Pause Game");
            PlayerData.SaveUserData();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Is Quit Game");
        PlayerData.SaveUserData();
    }
    #endregion

    #region Public Method

    #endregion
}
