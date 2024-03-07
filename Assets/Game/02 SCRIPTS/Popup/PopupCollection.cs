using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PopupCollection : SingletonPopup<PopupCollection>
{
    [Header("Data Input")]
    [SerializeField] SpriteAtlas atlasBG;
    [SerializeField] SpriteAtlas atlasBall;
    [SerializeField] CollectionSkinDataSO /*/*_tubeDataSO*/ _ballDataSO, _themeDataSo;
    [Header("REFFERENCE")]
    [SerializeField] ItemCollection _itemBallPrefab, _itemThemePrefab;
    [SerializeField] Transform[] _tabTrans;
    [SerializeField] TabButtonBase[] _btnTrans;
    [SerializeField] Sprite spr, def;
    [SerializeField] Color[] _textColorArr;

    public void Show()
    {
        base.Show();
    }

    private void Start()
    {
        Init();
        OnClickCollection(0);
    }

    public void Close()
    {
        base.Hide();
    }

    public void Init()
    {
        //for (int i = 0; i < atlasTube.spriteCount; i++)
        //{
        //    ItemCollection item = Instantiate(_itemTubePrefab, _tabTrans[0]);

        //    DataItemSkin data = new DataItemSkin(i, i < 4 ? 0 : 200 * i, _tubeDataSO.dataItemSkins[i].LevelUnlock);
        //    item.Init(data, atlasTube.GetSprite($"Bottle{i + 1:00}"));
        //}

        for (int i = 0; i < atlasBall.spriteCount; i++)
        {
            ItemCollection item = Instantiate(_itemBallPrefab, _tabTrans[0]);
            DataItemSkin data = new DataItemSkin(i, i < 6 ? 0 : 150 * i, _ballDataSO.dataItemSkins[i].LevelUnlock);
            item.Init(data, atlasBall.GetSprite($"top" + $"{i + 1:00}"));
        }

        for (int i = 0; i < atlasBG.spriteCount; i++)
        {
            ItemCollection item = Instantiate(_itemThemePrefab, _tabTrans[1]);
            DataItemSkin data = new DataItemSkin(i, i < 8 ? 0 : 100 * i, _themeDataSo.dataItemSkins[i].LevelUnlock);
            item.Init(data, atlasBG.GetSprite($"BG_" + $"{i + 1:00}"));
        }
    }

    public void OnClickCollection(int index)
    {
        for (int i = 0; i < _tabTrans.Length; i++)
        {
            _tabTrans[i].parent.gameObject.SetActive(i == index);
            if (i == index)
            {
                _btnTrans[i].OnClickTabChangeImg(spr, _textColorArr[0]);
            }
            else
            {
                _btnTrans[i].OnClickTabChangeImg(def, _textColorArr[1]);
            }
        }
    }

    public void OnClickRemoveAds()
    {
        // PopupRemoveAds.Instance.Show();
    }
}
