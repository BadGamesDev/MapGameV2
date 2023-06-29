using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EconomyUI : MonoBehaviour
{
    public GameState gameState;

    public Slider sliderTaxLevel;
    public Slider sliderDevBudget;

    //public TMP_Text textTaxLevel;
    //public TMP_Text textDevBudget;

    public void OnTaxationSliderChanged()
    {
        gameState.playerNation.taxLevel = sliderTaxLevel.value * 100f;
    }

    public void OnDevelopmentSliderChanged()
    {
        gameState.playerNation.developmentBudget = sliderDevBudget.value * 100f;
    }
}
