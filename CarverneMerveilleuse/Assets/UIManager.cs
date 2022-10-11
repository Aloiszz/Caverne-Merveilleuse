using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerController player;
    public Image healthBar;
    public TMP_Text moneyGet;
    public TMP_Text goldenGet;
    public int goldenMoney;
    public int money;

    private void Update()
    {
        healthBar.fillAmount = player.life / player.lifeDepard;
        moneyGet.SetText(money.ToString());
        goldenGet.SetText(goldenMoney.ToString());
        if (Input.GetKeyDown(KeyCode.M))
        {
            money += 10;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            goldenMoney += 1;
        }
    }
}
