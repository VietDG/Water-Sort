using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TypeBooster
{
    Back,
    Tube,
}
public class BoosterController : MonoBehaviour
{
    public TypeBooster Type;
    [SerializeField] TMP_Text _amoutText;
    [SerializeField] GameObject _iconAds;
    [SerializeField] GameObject _iconNumber;

    public void DisPlayBooster(int value)
    {
        _amoutText.text = value.ToString();
        if (value > 0)
        {
            _iconNumber.SetActive(true);
            _iconAds.SetActive(false);
        }
        else
        {
            _iconAds.SetActive(true);
            _iconNumber.SetActive(false);
        }
    }
}
