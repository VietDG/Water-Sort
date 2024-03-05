using DG.Tweening;
using PopupSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCallToRate : SingletonPopup<PopupCallToRate>
{
    [Header("---UI REFERENCES---")]
    [SerializeField] private RectTransform rTButtonRate;
    [SerializeField] private GameObject objButtonRate;
    [SerializeField] private Action Callback;
    [SerializeField] Image _emojiImg;
    [SerializeField] Sprite[] _emojiRate;
    public int CurrentStar;
    public Action<int> EvtUpdateRateStarStatus;

    #region Public Methods
    public void Show(Action callback = null)
    {
        SoundManager.Instance.PlayUIButtonClick();
        Callback = callback;
        base.Show();
        //    OnShowUI();
    }

    public void Close()
    {
        base.Hide(() => { Callback?.Invoke(); });
    }

    public void ShowButtonRate()
    {
        if (!objButtonRate.activeSelf)
        {
            rTButtonRate.localScale = Vector2.zero;
            objButtonRate.SetActive(true);
            rTButtonRate.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }        /*.OnComplete(() =>
          {
              rTButtonRate.DOScale(1.1f, 0.35f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
          })*/
    }

    public void Rate()
    {
        SoundManager.Instance.PlayUIButtonClick();
        base.Hide(() =>
        {
            if (CurrentStar >= 4)
            {
                GooglePlayInAppManager.Instance.Review();
                //PlayerPrefs.SetInt(Const.GAME_RATE_COMPLETED, 1);
                PlayerData.UserData.ValueToRate = CurrentStar;
            }
            Callback?.Invoke();
        });
    }

    public void DisplayEmoji(int id)
    {
        _emojiImg.sprite = _emojiRate[id];
    }
    #endregion
}

