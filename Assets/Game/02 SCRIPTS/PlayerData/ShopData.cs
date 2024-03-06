using System.Collections;
using System.Collections.Generic;

public class ShopData
{
    public List<UserItemCollection> TubeDatas = new List<UserItemCollection>();
    public List<UserItemCollection> BallDatas = new List<UserItemCollection>();
    public List<UserItemCollection> ThemeDatas = new List<UserItemCollection>();

    public UserItemCollection getItemCol(TypeSkinCollection type, int id)
    {
        return getListItemCol(type)[id];
    }

    public void BuyItem(TypeSkinCollection type, int id)
    {
        getListItemCol(type)[id].IsUnlock = true;

        EquipItem(type, id, true);
    }

    public void EquipItem(TypeSkinCollection type, int id, bool value)
    {
        getListItemCol(type)[id].isEquip = value;
    }

    private List<UserItemCollection> getListItemCol(TypeSkinCollection type)
    {
        switch (type)
        {
            case TypeSkinCollection.Tube:
                return TubeDatas;
            case TypeSkinCollection.Ball:
                return BallDatas;
            case TypeSkinCollection.Theme:
                return ThemeDatas;
            default:
                return null;
        }
    }

    public UserItemCollection getItemEquip(TypeSkinCollection type)
    {
        foreach (var item in getListItemCol(type))
        {
            if (!item.isEquip) continue;
            return item;
        }
        return null;
    }
}

[System.Serializable]
public class UserItemCollection
{
    public int Index;
    public bool IsUnlock;
    public bool isEquip;

    public UserItemCollection(int index, bool isBought, bool isEquit)
    {
        Index = index;
        IsUnlock = isBought;
        this.isEquip = isEquit;
    }
}