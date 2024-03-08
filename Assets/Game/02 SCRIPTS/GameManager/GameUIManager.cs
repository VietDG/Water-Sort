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
    [SerializeField] GameObject _displayLevel;
    [SerializeField] GameObject[] _btnGameUI;
    [Header("TUTORIAL")]
    [SerializeField] TMP_Text _tutTxt;

    public override void Awake()
    {
        ActionEvent.OnResetGamePlay += DisPlayLevelText;

        ActionEvent.OnSetSkin += DisPlayTheme;
    }

    private void Start()
    {
        DisPlayLevelText();
    }

    private void OnDestroy()
    {
        ActionEvent.OnResetGamePlay -= DisPlayLevelText;
        ActionEvent.OnSetSkin -= DisPlayTheme;
    }

    private void DisPlayLevelText()
    {
        _levelTxt.text = $"{PlayerData.UserData.HighestLevel + 1:00}";
        DisPlayTheme(TypeSkinCollection.Theme);
        DisplayGameUI(true);
    }

    private void DisPlayTheme(TypeSkinCollection type)
    {
        if (type.Equals(TypeSkinCollection.Theme))
        {
            _bg.sprite = GameManager.Instance.getBG();
        }
    }

    public void OnClickSetting()
    {
        PopupSetting.Instance.Show();
    }

    public void DisplayGameUI(bool isValue)
    {
        foreach (var btn in _btnGameUI)
        {
            btn.SetActive(isValue);
        }

        _displayLevel.SetActive(isValue);
    }

    public void DisplayTut(string value, bool isValue)
    {
        _tutTxt.text = value;
        _tutTxt.gameObject.SetActive(isValue);
    }
}

