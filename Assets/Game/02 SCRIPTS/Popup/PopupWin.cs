using PopupSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWin : SingletonPopup<PopupWin>
{
    public void Show()
    {
        base.Show();
    }

    public void Close()
    {
        base.Hide();
    }

    public void OnClickNextlevel()
    {
        ActionEvent.OnResetGamePlay?.Invoke();
        Close();
    }
}
