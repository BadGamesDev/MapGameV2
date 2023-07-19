using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EconomyUI : MonoBehaviour
{
    public GameState gameState;
    public MainUI mainUI;

    public Slider sliderTaxLevel;
    public Slider sliderDevBudget;
    public Slider sliderMilBudget;

    public TMP_Text taxIncomeText;
    public TMP_Text devExpenseText;
    public TMP_Text milExpenseText;
    public TMP_Text intExpenseText;

    public TMP_Text TotalIncomeText;
    public TMP_Text TotalExpenseText;

    public void UpdateEconomyUI() // I can just do this in update manager instead.
    {
        taxIncomeText.text = mainUI.FormatNumberMoney(gameState.playerNation.incomeTax);
        devExpenseText.text = mainUI.FormatNumberMoney(gameState.playerNation.expenseDev);
        milExpenseText.text = mainUI.FormatNumberMoney(gameState.playerNation.expenseMil);
        intExpenseText.text = mainUI.FormatNumberMoney(gameState.playerNation.expenseInt);

        TotalIncomeText.text = mainUI.FormatNumberMoney(gameState.playerNation.income);
        TotalExpenseText.text = mainUI.FormatNumberMoney(gameState.playerNation.expense);
    }

    public void OnTaxationSliderChanged()
    {
        gameState.playerNation.taxLevel = sliderTaxLevel.value;
    }

    public void OnDevelopmentSliderChanged()
    {
        gameState.playerNation.developmentBudget = sliderDevBudget.value;
    }

    public void OnMilitarySliderChanged()
    {
        gameState.playerNation.militaryBudget = sliderDevBudget.value;
    }
}
