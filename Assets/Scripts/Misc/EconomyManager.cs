using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    const string COIN_AMOUNT_TEXT = "GoldAmountText";
    private TMP_Text goldText;
    private int currentGold = 0;

    public void IncrementCurrentGold()
    {
        currentGold += 1;

        if(goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        // D3 overload will make sure the text always has three digits
        goldText.text = currentGold.ToString("D3");
    }
}
