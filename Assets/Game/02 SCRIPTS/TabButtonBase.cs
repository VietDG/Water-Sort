using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabButtonBase : MonoBehaviour
{
    [SerializeField] Image _bgImg;
    [SerializeField] TMP_Text _tileTxt;
    [SerializeField] GameObject _notiObj;

    public void OnClickTabChangeImg(Sprite spr, Color color)
    {
        _bgImg.sprite = spr;
        _tileTxt.color = color;
    }

    public virtual void OnActiveNoti(bool value)
    {
        _notiObj.SetActive(value);
    }
}
