using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : SingletonPopup<PopupSetting>
{
    [SerializeField] Toggle _sfx, _vibrate;
    public void Show()
    {
        base.canCloseWithOverlay = true;
        base.Show();
    }

    public void Close()
    {
        base.Hide();
    }

    private void Start()
    {
        SetUpICon();
    }

    #region Sound
    public void OnClickSound()
    {
        if (Setting.Instance.SOUND)
        {
            _sfx.isOn = true;
        }
        else
        {
            _sfx.isOn = false;
        }
        Setting.Instance.SOUND = !Setting.Instance.SOUND;
        SoundManager.Instance.SoundHandle(Setting.Instance.SOUND);
    }

    public void OnClickVibrate()
    {
        if (Setting.Instance.VIBRATE)
        {
            _vibrate.isOn = true;
        }
        else
        {
            _vibrate.isOn = false;
        }
        Setting.Instance.VIBRATE = !Setting.Instance.VIBRATE;
    }

    public void SetUpICon()
    {
        if (Setting.Instance.SOUND)
        {
            _sfx.isOn = false;
        }
        else
        {
            _sfx.isOn = true;
        }

        if (Setting.Instance.VIBRATE)
        {
            _vibrate.isOn = false;
        }
        else
        {
            _vibrate.isOn = true;
        }
    }
    #endregion

    public void OnclickCollection()
    {
        base.Hide(() =>
        {
            // PopupCollection.Instance.Show();
        });
    }

    public void OnClickLanguage()
    {
        //  ActionEvent.OnShowToast?.Invoke("Coming soon!");
    }
}
