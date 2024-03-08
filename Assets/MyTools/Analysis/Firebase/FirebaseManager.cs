using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : SingletonMonoBehaviour<FirebaseManager>
{
    private AppConfig _appConfig;
    private const string TAG = "[FIREBASE]";
    public bool _isInitialized;

    public override void Awake()
    {
        GlobalEventManager.EvtSendEvent += SendEvent;
        //  GlobalEventManager.Instance.EvtUpdateUserProperties += SetUserPropeties;
    }
    private void Start()
    {
        _appConfig = AppConfig.Instance;
        Init();
    }

    private void OnDestroy()
    {
        GlobalEventManager.EvtSendEvent -= SendEvent;
        //  GlobalEventManager.Instance.EvtUpdateUserProperties -= SetUserPropeties;
    }

    public void Init()
    {
        // Debug.Log($"{TAG} Init...");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                _isInitialized = true;
                FetchValue();
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                // Notifacation
                //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
                //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
                //  Debug.Log($"{TAG} Init done!");
            }
            else
            {
                // Debug.Log($"{TAG} Init fail!");
            }
        });
    }

    #region Firebase Analytis
    private void SendEvent(string eName, Parameter[] parameters)
    {
        if (!_isInitialized)
            return;
        if (parameters == null)
        {
            FirebaseAnalytics.LogEvent(eName);
            //Debug.LogError($"{eName}");
        }
        else
        {
            FirebaseAnalytics.LogEvent(eName, parameters);
            // Debug.LogError($"{eName}_{parameters}");
        }
    }
    #endregion

    #region Firebase Messaging
    //public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    //{
    //    Debug.Log("Received Registration Token: " + token.Token);
    //}

    //public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    //{
    //    Debug.Log("Received a new message from: " + e.Message.From);
    //}
    #endregion

    #region Firebase Remote Config
    private void FetchValue()
    {
        TimeSpan time = new TimeSpan(0, 0, 10);
        FirebaseRemoteConfig.DefaultInstance.FetchAsync(time).ContinueWithOnMainThread(task =>
        {
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            if (info.LastFetchStatus == LastFetchStatus.Success)
            {
                //  Debug.Log($"{TAG}: Read remote config...");
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync();

                _appConfig.BannerAdLevel = GetFetchIntValue(Const.KEY_BANNER_AD_LEVEL, _appConfig.BannerAdLevel);
                _appConfig.InterAdLevel = GetFetchIntValue(Const.KEY_INTER_AD_LEVEL, _appConfig.InterAdLevel);
                _appConfig.InterFrequencyTime = GetFetchIntValue(Const.KEY_INTER_FREQUENCY_TIME, _appConfig.InterFrequencyTime);
                _appConfig.IsShowInterWithClosePopupRemoveAds = GetFetchBoolValue(Const.KEY_INTER_AD_CLOSE_POPUP_REMOVE_ADS, _appConfig.IsShowInterWithClosePopupRemoveAds);
            }
        });
    }

    private int GetFetchIntValue(string valueName, int defaultValue)
    {
        string value = FirebaseRemoteConfig.DefaultInstance.GetValue(valueName).StringValue;
        if (value != null)
        {
            //Debug.Log(value);
            return int.Parse(value);
        }
        else
        {
            return defaultValue;
        }
    }

    private string GetFetchStringValue(string valueName, string defaultValue)
    {
        string value = FirebaseRemoteConfig.DefaultInstance.GetValue(valueName).StringValue;
        if (value != null)
        {
            // Debug.Log(value);
            return value;
        }
        else
        {
            return defaultValue;
        }
    }

    public bool GetFetchBoolValue(string valueName, bool defaultValue)
    {
        bool value = FirebaseRemoteConfig.DefaultInstance.GetValue(valueName).BooleanValue;

        if (value != defaultValue)
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }
    #endregion
}