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
            SaveAllData();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Is Quit Game");
        //   SaveAllData();
    }
    #endregion

    #region Public Method
    public void SaveAllData()
    {
        PlayerData.SaveUserData();
        CollectionData.SaveData();
    }


    #endregion
}
