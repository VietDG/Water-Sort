using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    [SerializeField] TMP_Text _valueCoinTxt;

    private void Awake()
    {
        ActionEvent.OnUpdateCoin += DisplayCoin;
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayCoin();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            EarnCoin(1000);
        }
    }

    private void OnDestroy()
    {
        ActionEvent.OnUpdateCoin -= DisplayCoin;
    }

    private void DisplayCoin()
    {
        _valueCoinTxt.text = PlayerData.UserData.Coin.ToString();
    }

    private void EarnCoin(int value)
    {
        PlayerData.UserData.EarnCoin(value);
    }

    public void OnClickAddCoin()
    {
        PopupAddCoin.Instance.Show();
    }
}
