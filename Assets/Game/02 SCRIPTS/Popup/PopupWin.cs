using DG.Tweening;
using PopupSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PopupWin : SingletonPopup<PopupWin>
{
    [Header("REFFERENCE")]
    [SerializeField] RewardDataSO _rewardDataSO;
    [SerializeField] CollectionSkinDataSO _tubeDataSO, _ballDataSO, _themeDataSO;
    [SerializeField] SpriteAtlas atlasBG;
    [SerializeField] SpriteAtlas atlasBall/*, atlasTube*/;
    [SerializeField] Progress _processReward;
    [SerializeField] Progress _processUnlockSkin;
    [SerializeField] Image _chestImg, _skinUnlockImg;
    [SerializeField] RectTransform _nextBtnRect;
    [SerializeField] Sprite _iconOpenChest;
    [SerializeField] Transform[] _starTrans;
    [SerializeField] Transform _victoryTxtTrans;
    /// <summary>
    /// Value
    /// </summary>
    private DataItemSkin _DataItemSkin;
    private bool _canShowPopupUnlockSkin;
    private float _duration = 0.3f;

    public void Show()
    {
        base.canCloseWithOverlay = true;
        DisplayUI();
        base.Show(() =>
        {
            DisplayAnimUI(() =>
            {
                CallProcessReward();

                CallProcessUnlockSkin();
            });

        });
    }

    public void Close()
    {
        base.Hide();
    }

    public void OnClickNextLevel()
    {
        // GameController.Instance.OnClickRestart();
        base.Hide(() =>
        {
            ActionEvent.OnResetGamePlay?.Invoke();
            //if (GameManager.Instance.Level.level % 5 == 0 && PlayerData.UserData.ValueToRate <= 0)
            //{
            //    PopupCallToRate.Instance.Show();
            //}
        });
    }

    private void DisplayUI()
    {
        _nextBtnRect.gameObject.SetActive(false);

        DisplayProcessReward();

        DisplayProcessUnlockSkin();

        foreach (var star in _starTrans)
        {
            star.localScale = Vector2.zero;
            star.Rotate(0, 0, 45);
        }
        _victoryTxtTrans.localScale = Vector2.zero;

        _processReward.transform.localScale = Vector2.zero;
        _processUnlockSkin.transform.localScale = Vector2.zero;
    }

    private void DisplayProcessReward()
    {
        int valueMin = PlayerData.UserData.CurrentRewardValue;
        int valueMax = _rewardDataSO.getValueMax(PlayerData.UserData.RewardValueMax);

        _processReward.UpdateInfor($"Reach Level {valueMax}");

        _processReward.UpdateProcess(valueMin, valueMax);
    }

    private void CallProcessReward()
    {
        PlayerData.UserData.CurrentRewardValue.RefIntcrement();
        int tmp = PlayerData.UserData.CurrentRewardValue;
        _processReward.transform.DOScale(1, _duration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _processReward.CallProcess(tmp, (n) =>
            {
                DisplayButton(_DataItemSkin is null);
                if (n)
                {
                    _canShowPopupUnlockSkin = _processUnlockSkin.isMaxProcess(PlayerData.UserData.HighestLevel);

                    PopupChestReward.Instance.Show(_canShowPopupUnlockSkin);
                    _chestImg.sprite = _iconOpenChest;
                    PlayerData.UserData.CurrentRewardValue.RefIntReset();
                    if (PlayerData.UserData.RewardValueMax >= _rewardDataSO.MaxProcess()) return;
                    PlayerData.UserData.RewardValueMax.RefIntcrement();
                }
                else
                {

                }
            });
        });
    }

    private void DisplayProcessUnlockSkin()
    {
        _DataItemSkin = getLevelSkinUnlock();
        if (_DataItemSkin is null)
        {
            _processUnlockSkin.gameObject.SetActive(false);
            return;
        }

        int valueMin = PlayerData.UserData.HighestLevel - 1;
        int valueMax = _DataItemSkin.LevelUnlock;

        _processUnlockSkin.UpdateInfor($"Reach Level {valueMax}");

        _processUnlockSkin.UpdateProcess(valueMin, valueMax);

        //   _skinUnlockImg.sprite = getIconSkin(_DataItemSkin.Type, _DataItemSkin.Index);
    }

    private void CallProcessUnlockSkin()
    {
        int tmp = PlayerData.UserData.HighestLevel;
        _processUnlockSkin.transform.DOScale(1, _duration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            _processUnlockSkin.CallProcess(tmp, (n) =>
            {
                DisplayButton(true);

                if (n)
                {
                    if (_canShowPopupUnlockSkin)
                    {
                        return;
                    }
                    ShowPopupUnlockSkin();
                }
                else
                {
                }
            });
        });
    }

    private DataItemSkin getLevelSkinUnlock()
    {
        List<DataItemSkin> list = new List<DataItemSkin>();

        foreach (var tube in _tubeDataSO.dataItemSkins)
        {
            if (tube.LevelUnlock <= 0 || tube.Price > 0) continue;

            if (PlayerData.UserData.HighestLevel - 1 < tube.LevelUnlock)
            {
                list.Add(tube);
                //  Debug.Log($"level Unlock Tube: {tube.LevelUnlock} ");
                break;
            }
        }
        foreach (var ball in _ballDataSO.dataItemSkins)
        {
            if (ball.LevelUnlock <= 0 || ball.Price > 0) continue;

            if (PlayerData.UserData.HighestLevel - 1 < ball.LevelUnlock)
            {
                list.Add(ball);
                // Debug.Log($"Level unlock ball: {ball.LevelUnlock}");
                break;
            }
        }

        foreach (var theme in _themeDataSO.dataItemSkins)
        {
            if (theme.LevelUnlock <= 0 || theme.Price > 0) continue;

            if (PlayerData.UserData.HighestLevel - 1 < theme.LevelUnlock)
            {
                list.Add(theme);
                //  Debug.Log($"Level unlock theme: {theme.LevelUnlock}");
                break;
            }
        }

        int num = list.Count;
        if (num == 0)
        {
            return null;
        }

        DataItemSkin tmp = list[0];

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].LevelUnlock < tmp.LevelUnlock)
            {
                tmp = list[i];
            }
        }
        return tmp;
    }

    //private Sprite getIconSkin(TypeSkinCollection type, int id)
    //{
    //    //switch (type)
    //    //{
    //    //    case TypeSkinCollection.Tube:
    //    //        return atlasTube.GetSprite($"Ui_Rewards_Icon_Card_{id + 1:00}");
    //    //    case TypeSkinCollection.Ball:
    //    //        return atlasBall.GetSprite($"Ui_Shop_Ball{id + 1:00}");
    //    //    case TypeSkinCollection.Theme:
    //    //        return atlasBG.GetSprite($"Ui_Shop_Theme{id + 1:00}_B");
    //    //    default:
    //    //        return null;
    //    //}
    //}

    private void DisplayButton(bool value)
    {
        _nextBtnRect.gameObject.SetActive(value);
        if (!value) return;
        _nextBtnRect.localScale = Vector2.zero;
        _nextBtnRect.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }

    private void DisplayAnimUI(Action callBack)
    {
        Sequence sequence = DOTween.Sequence();
        foreach (var star in _starTrans)
        {
            sequence.Append(star.DORotate(Vector3.zero, 0.3f));
            sequence.Join(star.DOScale(1, 0.3f).SetEase(Ease.OutQuad));
        }
        sequence.Append(_victoryTxtTrans.DOScale(1, 0.3f).SetEase(Ease.OutQuad)).OnComplete(() =>
        {
            callBack?.Invoke();
        });
    }

    public void ShowPopupUnlockSkin()
    {
        //   PopupUnlockSkin.Instance.Show(_DataItemSkin, _skinUnlockImg.sprite);
    }
}
