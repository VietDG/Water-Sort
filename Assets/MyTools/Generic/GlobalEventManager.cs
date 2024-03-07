﻿using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEventManager
{
    public static Action<string, Parameter[]> EvtSendEvent;

    #region Tracking Ads
    public static void OnShowInterstitial()
    {
        EvtSendEvent?.Invoke("inter_show", null);
    }

    public static void OnCloseInterstitial()
    {
        EvtSendEvent?.Invoke("inter_close", null);
    }

    public static void OnShowRewarded(int level)
    {
        Parameter[] parameter = new Parameter[]
     {
             new Parameter("level", level.ToString()),
     };
        EvtSendEvent?.Invoke("reward_show", parameter);
    }

    public static void OnRewardedComplete(int level)
    {
        Parameter[] parameter = new Parameter[]
  {
             new Parameter("level", level.ToString()),
  };
        EvtSendEvent?.Invoke("reward_complete", parameter);
    }
    #endregion

    #region Tracking GamePlay
    public static int FirstPlayLevelTracking
    {
        get => PlayerPrefs.GetInt("first_play_level", 0);
        set => PlayerPrefs.SetInt("first_play_level", value);
    }

    public static void OnLevelPlay(int level)
    {
        if (FirstPlayLevelTracking == level) return;
        Parameter[] parameter = new Parameter[]
       {
             new Parameter("level", level.ToString()),
       };
        FirstPlayLevelTracking = level;
        EvtSendEvent?.Invoke($"level_play", parameter);
    }

    public static void OnLevelComplete(int level)
    {
        Parameter[] parameter = new Parameter[]
      {
             new Parameter("level", level.ToString()),
      };
        EvtSendEvent?.Invoke($"level_Win", parameter);
    }

    public static void OnLevelReplay(int level)
    {
        Parameter[] parameter = new Parameter[]
      {
             new Parameter("level", level.ToString()),
      };
        EvtSendEvent?.Invoke($"level_replay", parameter);
    }
    #endregion
}
