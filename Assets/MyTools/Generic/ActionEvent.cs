using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEvent : MonoBehaviour
{
    public delegate void NoParam();
    public delegate void OneParam(object obj);
    public delegate void TwoParam(object obj, object obj1);
    public delegate void MultiParam(object[] objs);

    #region GamePlay
    public static Action OnResetGamePlay;
    public static Action OnUserBoosterBack;
    public static Action OnUserBoosterAddTube;
    #endregion
}
