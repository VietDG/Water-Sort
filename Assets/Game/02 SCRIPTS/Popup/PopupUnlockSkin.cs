using DG.Tweening;
using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUnlockSkin : SingletonPopup<PopupUnlockSkin>
{
    [SerializeField] private Image _iconSkinImag;
    [SerializeField] RectTransform _itemSkinTrans;
    [SerializeField] Transform _btnUseTrans, _btnContinueTrans, _rewardTxtTrans;
    private DataItemSkin _skinData;
    Vector2 _originalItemSkinPos;

    public void Show(DataItemSkin skinData, Sprite sprite)
    {
        base.Show();

        _iconSkinImag.sprite = sprite;
        _skinData = skinData;
        SoundManager.Instance.PlaySfxRewind(GlobalSetting.GetSFX("reward_shining"));

        HideUI();

        ShowUI();
    }

    public void Close()
    {
        base.Hide();
    }

    private void HideUI()
    {
        _btnUseTrans.localScale = Vector2.zero;
        _btnContinueTrans.localScale = Vector2.zero;
        _rewardTxtTrans.localScale = Vector2.zero;

        _originalItemSkinPos = _itemSkinTrans.anchoredPosition;
        _itemSkinTrans.anchoredPosition = new Vector2(-GlobalSetting.getWidthUI(PopupManager.Instance.canvas) - _itemSkinTrans.sizeDelta.x, _originalItemSkinPos.y);
    }

    private void ShowUI()
    {
        _itemSkinTrans.DOAnchorPosX(_originalItemSkinPos.x, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _rewardTxtTrans.DOScale(1, 0.5f).SetEase(Ease.OutQuad);
            _btnUseTrans.DOScale(new Vector3(0.8f, 0.8f, 1), 0.5f).SetEase(Ease.OutBack);
            _btnContinueTrans.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetDelay(1f);
        });
    }

    public void OnClickClaim()
    {
        bool _isShow = false;
        AdsManager.Instance.ShowRewardedAd(() =>
        {
            _isShow = true;
        }, () =>
        {
            if (_isShow)
            {
                CollectionData.ShopData.getItemEquip(_skinData.Type).isEquip = false;
                CollectionData.ShopData.BuyItem(_skinData.Type, _skinData.Index);
                ActionEvent.OnSetSkin?.Invoke(_skinData.Type);
                Close();
            }
            else
            {
                ActionEvent.OnShowToast?.Invoke(Const.KEY_ERROR_ADS);
            }
        });
    }
}
