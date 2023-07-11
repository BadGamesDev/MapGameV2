using System.Collections;
using System.Collections.Generic;
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

    //public TMP_Text textTaxLevel;
    //public TMP_Text textDevBudget;

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
