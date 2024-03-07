using Newtonsoft.Json;
using Spine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CollectionData : MonoBehaviour
{
    public static ShopData ShopData = new ShopData();

    private void Start()
    {
        //******COLLECTION DATA***********
        InitData();
    }

    private void InitData()
    {
        if (!PlayerPrefs.HasKey(Const.KEY_USER_COLLECTION_DATA))
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    UserItemCollection item;
            //    if (i == 0)
            //    {
            //        item = new UserItemCollection(i, true, true);
            //    }
            //    else
            //    {
            //        item = new UserItemCollection(i, false, false);
            //    }
            //    ShopData.TubeDatas.Add(item);
            //}

            for (int i = 0; i < 9; i++)
            {
                UserItemCollection item;
                if (i == 0)
                {
                    item = new UserItemCollection(i, true, true);
                }
                else
                {
                    item = new UserItemCollection(i, false, false);
                }
                ShopData.BallDatas.Add(item);
            }

            for (int i = 0; i < 11; i++)
            {
                UserItemCollection item;
                if (i == 0)
                {
                    item = new UserItemCollection(i, true, true);
                }
                else
                {
                    item = new UserItemCollection(i, false, false);
                }
                ShopData.ThemeDatas.Add(item);
            }

            SaveData();
            if (!File.Exists(PlayerData.path + "/CollectionData.txt"))
            {
                // Create a file to write to.
                File.Create(PlayerData.path + "/CollectionData.txt");
            }
        }
        else
        {
            LoadData();
        }
    }

    #region Save && Load
    public static void LoadData()
    {
        var saveData = PlayerPrefs.GetString(Const.KEY_USER_COLLECTION_DATA);
        var data = JsonConvert.DeserializeObject<ShopData>(saveData);
        ShopData = data;
    }

    public static void SaveData()
    {
        string saveData = JsonConvert.SerializeObject(ShopData, Formatting.Indented);
        PlayerPrefs.SetString(Const.KEY_USER_COLLECTION_DATA, saveData);
        File.WriteAllText(PlayerData.path + "/CollectionData.txt", saveData);
    }
    #endregion
}
