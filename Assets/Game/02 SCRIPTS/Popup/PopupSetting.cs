using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : SingletonPopup<PopupSetting>
{
    [SerializeField] Toggle _sfx, _vibrate;

    private Setting _setting => Setting.Instance;

    public void Show()
    {
        base.Show();
    }

    public void Close()
    {
        base.Hide();
    }

    private void Start()
    {
        SetUpIcon();
    }

    #region Sound
    public void OnClickSound()
    {
        if (_setting.SOUND)
        {
            _sfx.isOn = true;
        }
        else
        {
            _sfx.isOn = false;
        }
        _setting.SOUND = !_setting.SOUND;
        SoundManager.Instance.SoundHandle(_setting.SOUND);
    }

    public void OnClickVibrate()
    {
        if (_setting.VIBRATE)
        {
            _vibrate.isOn = true;
        }
        else
        {
            _vibrate.isOn = false;
        }
        _setting.VIBRATE = !_setting.VIBRATE;
    }

    public void SetUpIcon()
    {
        if (_setting.SOUND)
        {
            _sfx.isOn = false;
        }
        else
        {
            _sfx.isOn = true;
        }

        if (_setting.VIBRATE)
        {
            _vibrate.isOn = false;
        }
        else
        {
            _vibrate.isOn = true;
        }
    }
    #endregion
}
