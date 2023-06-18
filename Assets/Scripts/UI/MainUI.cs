using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    public GameState gameState;

    public TMP_Text populationIndicator;
    public TMP_Text moneyIndicator;

    public TMP_InputField armyInfantrySizeInput;
    public TMP_InputField armyCavalrySizeInput;
    
    public Button armySizeDoneButton;

    public GameObject economyTab;


    private void Update()
    {
        if (gameState.playerNation != null)
        {
            populationIndicator.text ="Pop: " + FormatNumber(gameState.playerNation.population);
            moneyIndicator.text ="Money: " + FormatNumber(gameState.playerNation.money);
        }
    }

    public void OpenEconomyTab() // this is horrible UI, will fix it in the future
    {
        if (economyTab.activeSelf)
        {
            economyTab.SetActive(false);
        }
        else 
        {
            economyTab.SetActive(true);
        }
    }

    public string FormatNumber(int number)
    {
        const int Thousand = 1000;
        const int Million = 1000000;

        if (number < Thousand)
        {
            return number.ToString();
        }
        else if (number < Million)
        {
            float populationInK = number / (float)Thousand;
            return populationInK.ToString("0.00") + "K";
        }
        else
        {
            float populationInM = number / (float)Million;
            return populationInM.ToString("0.00") + "M";
        }
    }
}
