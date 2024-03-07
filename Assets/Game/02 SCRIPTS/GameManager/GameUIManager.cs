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
}

