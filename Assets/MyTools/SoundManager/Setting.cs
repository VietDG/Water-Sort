using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : SingletonMonoBehaviour<Setting>
{
    #region Member Varibales
    private bool _sound, _music, _vibrate;

    private const string KEY_SOUND = "setting_sound";
    // private const string KEY_MUSIC = "setting_music";
    private const string KEY_VIBRATE = "setting_vibrate";

    #endregion

    #region Public Methods

    public override void Awake()
    {
        base.Awake();
        SOUND = PlayerPrefs.GetInt(KEY_SOUND, 1) == 1;
        //   MUSIC = PlayerPrefs.GetInt(KEY_MUSIC, 1) == 1;
        VIBRATE = PlayerPrefs.GetInt(KEY_VIBRATE, 1) == 1;
    }
    public bool SOUND
    {
        get { return _sound; }
        set
        {
            _sound = value;
            PlayerPrefs.SetInt(KEY_SOUND, _sound ? 1 : 0);
        }
    }

    //public bool MUSIC
    //{
    //    get { return _music; }
    //    set
    //    {
    //        _music = value;
    //        PlayerPrefs.SetInt(KEY_MUSIC, _music ? 1 : 0);
    //    }
    //}

    public bool VIBRATE
    {
        get { return _vibrate; }
        set
        {
            _vibrate = value;
            PlayerPrefs.SetInt(KEY_VIBRATE, _vibrate ? 1 : 0);
        }
    }
    #endregion
}
