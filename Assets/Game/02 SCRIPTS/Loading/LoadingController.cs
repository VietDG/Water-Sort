using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text _loadingText;
    [SerializeField] private int _percent = 100;
    [SerializeField] private float _delayTime = 0.1f;
    [SerializeField] private Slider _loadingSlider;

    // Start is called before the first frame update
    void Start()
    {
        RunLoading();
    }

    private void RunLoading()
    {
        StartCoroutine(CounterTime.CounterUp(_percent, _delayTime, OnCouter, OnCounterComplete));
    }

    private void OnCouter(int value)
    {
        _loadingText.text = value > 100 ? $"100%" : $"{value}%";
        _loadingSlider.value = (float)value / _percent;
    }

    private void OnCounterComplete()
    {
        StartCoroutine(WaitCounterComplete());
    }

    private IEnumerator WaitCounterComplete()
    {
        yield return new WaitUntil(() => AdsManager.Instance.CanLoadAds);
        SceneManager.LoadScene(Const.SCENE_GAME);
    }
}