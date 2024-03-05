using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetting : SingletonMonoBehaviour<GlobalSetting>
{
    [SerializeField] GameObject _loadingAdsObj;

    public override void Awake()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
        DontDestroyOnLoad(gameObject);
    }
    public void Update()
    {

    }

    public static bool NetWorkRequirements()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    public static AudioClip GetSFX(string audioName)
    {
        // Debug.Log(Resources.Load<AudioClip>("SFX/" + audioName));
        return Resources.Load<AudioClip>("SFX/" + audioName);
    }

    public void ShowLoadingAd(Action callback)
    {
        StartCoroutine(LoadingAd(callback));
    }

    IEnumerator LoadingAd(Action callback)
    {
        _loadingAdsObj.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _loadingAdsObj.gameObject.SetActive(false);
        callback?.Invoke();
    }

    #region Global Method
    public static float getWidthUI(Canvas canvas)
    {
        float width = canvas.GetComponent<RectTransform>().anchoredPosition.x;
        return width;
    }

    public static float getHeightUI(Canvas canvas)
    {
        float heigh = canvas.GetComponent<RectTransform>().anchoredPosition.y;
        return heigh;
    }
    #endregion
}