using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToastController : MonoBehaviour
{
    [SerializeField] RectTransform _toastTrans;
    [SerializeField] TMP_Text _contentToastTxt;
    private float _duration = 0.5f, _delayTime = 1f;
    private float _startPosY;
    [SerializeField] float _endPosY;
    private bool _isShowingToast;

    private void Awake()
    {
        ActionEvent.OnShowToast += ShowToast;
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosY = _toastTrans.anchoredPosition.y;
    }

    //-----------------------------------------------------------------------------
    public void ShowToast(string content)
    {
        if (_isShowingToast) return;

        _isShowingToast = true;

        _contentToastTxt.text = content;

        _toastTrans.DOAnchorPosY(_endPosY, _duration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Invoke(nameof(HideToast), _delayTime);
        });
    }

    private void HideToast()
    {
        _toastTrans.DOAnchorPosY(_startPosY, _duration).SetEase(Ease.InBack).OnComplete(() =>
        {
            _isShowingToast = false;
        });
    }
}
