using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
#if UNITY_EDITOR
    public const string path = "Assets/Game/05 Data/JsonText/PlayerData";
#else
    public static string path = Application.persistentDataPath;
#endif
    public static UserData UserData = new UserData();

    private void Start()
    {
        InitData();
    }

    private void InitData()
    {
        //UserData
        if (!PlayerPrefs.HasKey(Const.KEY_USER_DATA))
        {
            SaveUserData();
        }
        else
        {
            LoadUserData();
        }
    }

    public static void LoadUserData()
    {
        var saveData = PlayerPrefs.GetString(Const.KEY_USER_DATA);
        var data = JsonUtility.FromJson<UserData>(saveData);
        UserData = data;
    }

    public static void SaveUserData()
    {
        string saveData = JsonUtility.ToJson(UserData);
        PlayerPrefs.SetString(Const.KEY_USER_DATA, saveData);

        File.WriteAllText(path + "/UserData.txt", saveData);
    }
}
