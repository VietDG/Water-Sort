using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TypeBooster
{
    Back,
    AddTube
}
public class BoosterController : MonoBehaviour
{
    public TypeBooster Type;
    [SerializeField] TMP_Text _amoutText;
    [SerializeField] GameObject _iconAds;

    public void DisPlayBooster(int value)
    {
        _amoutText.text = value.ToString();
        if (value > 0)
            _iconAds.SetActive(false);
        else
            _iconAds.SetActive(true);
    }
}
