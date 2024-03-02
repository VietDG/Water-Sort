using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : SingletonMonoBehaviour<GameUIManager>
{
    [Header("REFERENCE")]
    [SerializeField] TMP_Text _levelTxt;
    [SerializeField] Image _bg;

    public override void Awake()
    {
        ActionEvent.OnResetGamePlay += DisPlayLevelText;
    }

    private void Start()
    {
        DisPlayLevelText();
    }

    private void OnDestroy()
    {
        ActionEvent.OnResetGamePlay -= DisPlayLevelText;
    }

    private void DisPlayLevelText()
    {
        _levelTxt.text = $"{PlayerData.UserData.HighestLevel + 1:00}";

    }

    private void DisPlayTheme(TypeSkinColect type)
    {
        if (type.Equals(TypeSkinColect.Theme))
        {
            //  _bg.sprite = GameManager.Instance.get
        }

    }
}

public enum TypeSkinColect
{
    Tube = 0,
    Theme = 1,
    Ball = 2,
}
