using DG.Tweening;
using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupChestReward : SingletonPopup<PopupChestReward>
{
    [SerializeField] ChestController _chestController;
    [SerializeField] GameObject _ClaimX2Obj, _continueObj;
    [SerializeField] TMP_Text _valueRewardTxt;
    [SerializeField] Transform _containTrans;
    private RectTransform[] _rectTransformsItem;
    private Vector2[] _originalPosItems;
    [SerializeField] private Vector2 _originalItemDefault;
    [SerializeField] float _duration;
    private int _count;
    private bool _isShowPopupUnlockSkin;
    int _valueReward = 100;
    public void Show(bool isShowPopupUnlockSkin)
    {
        _isShowPopupUnlockSkin = isShowPopupUnlockSkin;
        _valueRewardTxt.text = $"x{_valueReward}";
        InitItem();
        base.Show(() =>
        {
            DisplayChest();
        });
        SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("reward_shining"));
    }

    public void Close()
    {
        base.Hide(() =>
        {
            if (_isShowPopupUnlockSkin)
            {
                PopupWin.Instance.ShowPopupUnlockSkin();
            }
            PlayerData.UserData.EarnCoin(_valueReward);
        });
    }

    private void InitItem()
    {
        _count = _containTrans.childCount;
        _originalPosItems = new Vector2[_count];
        _rectTransformsItem = new RectTransform[_count];
        for (int i = 0; i < _count; i++)
        {
            RectTransform recItem = _containTrans.GetChild(i).GetComponent<RectTransform>();
            _rectTransformsItem[i] = recItem;
            _originalPosItems[i] = recItem.anchoredPosition;
            recItem.anchoredPosition = _originalItemDefault;
            recItem.localScale = new Vector3(0, 0, recItem.localScale.z);
        }
    }

    private void DisplayChest()
    {
        _continueObj.transform.localScale = Vector2.zero;
        _ClaimX2Obj.transform.localScale = Vector2.zero;
        _chestController.InitChest(() =>
        {
            //do some thing
            WaitMovementItem();
        });
    }

    private void WaitMovementItem()
    {
        var sequence = DOTween.Sequence();
        for (int i = 0; i < _rectTransformsItem.Length; i++)
        {
            Vector2 targetPos = _originalPosItems[i];
            _rectTransformsItem[i].gameObject.SetActive(true);
            sequence.Append(_rectTransformsItem[i].DOAnchorPos(targetPos, _duration).SetEase(Ease.OutBack));
            sequence.Join(_rectTransformsItem[i].DOScale(new Vector3(1, 1, _rectTransformsItem[i].localScale.z), 0.5f).SetEase(Ease.OutBack));
        }
        sequence.Append(_ClaimX2Obj.transform.DOScale(new Vector3(0.8f, 0.8f, 1), 0.5f).SetEase(Ease.OutBack));
        sequence.Append(_continueObj.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f));
    }

    public void OnClickClaimX2Reward()
    {
        bool _isShow = false;
        AdsManager.Instance.ShowRewardedAd(() =>
        {
            _isShow = true;
        }, () =>
        {
            if (_isShow)
            {
                base.Hide(() =>
                {
                    if (_isShowPopupUnlockSkin)
                    {
                        PopupWin.Instance.ShowPopupUnlockSkin();
                    }
                    PlayerData.UserData.EarnCoin(_valueReward * 2);
                });
            }
            else
            {
                ActionEvent.OnShowToast?.Invoke(Const.KEY_ERROR_ADS);
            }
        });
    }
}
