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
    public Button recruitmentCancelButton;

    public GameObject economyTab;
    public GameObject tradeTab;

    public GameObject welcomeMessage; // THIS WILL BE DELETED! THIS IS JUST TEMPORARY!
    public GameObject settlementMessage; //Also there should only be one message window to display all messages(probably)

    public void UpdateMainDisplay() //method name sucks 
    {
        if (gameState.playerNation != null)
        {
            populationIndicator.text ="Population: " + FormatNumberPop(gameState.playerNation.population);
            moneyIndicator.text = "Treasury: " + FormatNumberMoney(gameState.playerNation.money);
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
        if (tradeTab.activeSelf)
        {
            tradeTab.SetActive(false);
        }
    }

    public void OpenTradeTab() // this is horrible UI, will fix it in the future
    {
        if (tradeTab.activeSelf)
        {
            tradeTab.SetActive(false);
        }
        else
        {
            tradeTab.SetActive(true);
        }
        if (economyTab.activeSelf)
        {
            economyTab.SetActive(false);
        }
    }

    public string FormatNumberPop(int number)
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

    public string FormatNumberMoney(float number)
    {
        const int Thousand = 1000;
        const int Million = 1000000;

        if (number < Thousand)
        {
            return number.ToString("0.##");
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

    public string FormatNumberResource(float number)
    {
        const int Thousand = 1000;
        const int Million = 1000000;

        if (number < Thousand)
        {
            return number.ToString("0.##");
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

    public void WelcomeMessageCloseButton() //make a generalised close button method.
    {
        welcomeMessage.SetActive(false); 
    }

    public void SettlementMessageCloseButton() //make a generalised close button method.
    {
        settlementMessage.SetActive(false);
    }
}
