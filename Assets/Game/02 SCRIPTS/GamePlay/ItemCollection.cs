using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataItemSkin
{
    public int Index;
    public TypeSkinCollection Type;
    public int Price;
    public int LevelUnlock;

    public DataItemSkin(int index, int price, int Levelunlock)
    {
        Index = index;
        Price = price;
        this.LevelUnlock = Levelunlock;
    }
}

public class ItemCollection : MonoBehaviour
{
    public DataItemSkin dataBall;
    [SerializeField] TypeSkinCollection type;
    [SerializeField] Image iconItem;
    [SerializeField] TMP_Text textLv, textPrice;
    [SerializeField] GameObject mask, iconReceive;
    [SerializeField] GameObject _buyWithCoinBtn, _buyWithAdsBtn;
    private Sprite _spr;
    private bool _isUnlock, _isEquip;

    private void OnEnable()
    {
        ActionEvent.OnChangeSkinEquip += ChangeEquip;
    }

    private void Start()
    {
        DisplayItem();
    }

    private void OnDisable()
    {
        ActionEvent.OnChangeSkinEquip -= ChangeEquip;
    }

    public void Init(DataItemSkin data, Sprite newSprite)
    {
        dataBall = data;
        _spr = newSprite;
    }

    public void DisplayItem()
    {
        iconItem.sprite = _spr;

        _isUnlock = CollectionData.ShopData.getItemCol(type, dataBall.Index).IsUnlock;

        _isEquip = CollectionData.ShopData.getItemCol(type, dataBall.Index).isEquip;

        mask.SetActive(!_isUnlock);
        if (_isUnlock)
        {
            DisplayItemUnlock();
        }
        else
        {
            textLv.gameObject.SetActive(dataBall.Price <= 0);

            textPrice.text = $"{dataBall.Price}";
            textLv.text = $"Lv. {dataBall.LevelUnlock}";

            if (dataBall.Price > 0)
            {
                _buyWithCoinBtn.SetActive(true);
                textLv.gameObject.SetActive(false);
                _buyWithAdsBtn.SetActive(false);
            }
            else
            {
                _buyWithCoinBtn.SetActive(false);
                if (PlayerData.UserData.HighestLevel >= dataBall.LevelUnlock)
                {
                    textLv.gameObject.SetActive(false);
                    _buyWithAdsBtn.SetActive(true);
                }
                else
                {
                    textLv.gameObject.SetActive(true);
                    _buyWithAdsBtn.SetActive(false);
                }
            }
        }
    }

    private void DisplayItemUnlock()
    {
        mask.SetActive(!_isUnlock);

        iconReceive.SetActive(_isEquip);

        _buyWithAdsBtn.SetActive(!_isUnlock);

        _buyWithCoinBtn.SetActive(!_isUnlock);
    }

    public void ChangeEquip()
    {
        if (!_isUnlock || !_isEquip) return;
        _isEquip = !_isEquip;
        iconReceive.SetActive(_isEquip);

        CollectionData.ShopData.EquipItem(type, dataBall.Index, _isEquip);
    }

    public void OnClickEquip()
    {
        if (!_isUnlock)
        {
            ActionEvent.OnShowToast?.Invoke(Const.KEY_NOT_UNLOCK_SKIN);
            return;
        }
        if (_isEquip) return;
        ActionEvent.OnChangeSkinEquip?.Invoke();
        _isEquip = true;
        iconReceive.SetActive(true);
        CollectionData.ShopData.EquipItem(type, dataBall.Index, _isEquip);
        ActionEvent.OnSelectSkin?.Invoke(type);
    }

    public void OnClickBuyWithCoin()
    {
        PlayerData.UserData.SpendCoin(dataBall.Price, (n) =>
        {
            if (n)
            {
                ActionEvent.OnUpdateCoin?.Invoke();
                ActionEvent.OnChangeSkinEquip?.Invoke();
                _isUnlock = true;
                _isEquip = true;
                CollectionData.ShopData.BuyItem(type, dataBall.Index);
                DisplayItemUnlock();
                ActionEvent.OnSelectSkin?.Invoke(type);
            }
            else
            {
                PopupAddCoin.Instance.Show();
            }
        });
    }

    public void OnClickBuyWithAds()
    {
        bool _isShow = false;
        AdsManager.Instance.ShowRewardedAd(() =>
        {
            _isShow = true;
        }, () =>
        {
            if (_isShow)
            {
                ActionEvent.OnChangeSkinEquip?.Invoke();
                _isUnlock = true;
                _isEquip = true;
                CollectionData.ShopData.BuyItem(type, dataBall.Index);
                DisplayItemUnlock();
                ActionEvent.OnSelectSkin?.Invoke(type);
            }
            else
            {
                ActionEvent.OnShowToast?.Invoke(Const.KEY_ERROR_ADS);
            }
        });
    }
}

public enum TypeSkinCollection
{
    Tube = 0,
    Theme = 1,
    Ball = 2,
}