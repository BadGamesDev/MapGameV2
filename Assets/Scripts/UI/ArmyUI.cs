﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ArmyUI : MonoBehaviour
{
    public GameState gameState;
    public ArmyProps armyProps;
    
    public GameObject panelUI;

    public Button buttonSetReinforce;
    public Button buttonEditReinforceTiles; //not a big fan of referencing it like this

    public TMP_Text armyOwner;

    //public TMP_Text recruitCount;

    public TMP_Text maxTroopCount;
    public TMP_Text maxInfantryCount;
    public TMP_Text maxCavalryCount;

    public TMP_Text curTroopCount;
    public TMP_Text infantryCount;
    public TMP_Text cavalryCount;

    public void OpenArmyUI()
    {
        if (!panelUI.activeSelf)
        {
            panelUI.SetActive(true);
        }
    }

    public void CloseArmyUI()
    {
        if (panelUI.activeSelf)
        {
            panelUI.SetActive(false);
        }
    }

    public void UpdateArmyUI()
    {
        armyOwner.text = gameState.activeArmy.nation.name; //Null reference error here!!! (Because UI is not closed when army is de-selected)

        //recruitCount.text = gameState.activeArmy.availablePop.ToString(); //Recruit count doesn't get updated as frequently as it should for some reason

        curTroopCount.text = gameState.activeArmy.curSize.ToString();
        infantryCount.text = gameState.activeArmy.curInfantry.ToString();
        cavalryCount.text = gameState.activeArmy.curCavalry.ToString();

        maxTroopCount.text = gameState.activeArmy.maxSize.ToString();
        maxInfantryCount.text = gameState.activeArmy.maxInfantry.ToString();
        maxCavalryCount.text = gameState.activeArmy.maxCavalry.ToString();
    }

    public void SetReinforcement() //Fınd a better name for this method
    {
        if(gameState.activeArmy.reinforce == true)
        {
            gameState.activeArmy.reinforce = false;
        }
        else
        {
            gameState.activeArmy.reinforce = true;
        }
    }

    public void EditReinforceTiles()
    {
        if (gameState.gameMode != GameState.Mode.recruitModeTiles)
        {
            gameState.gameMode = GameState.Mode.recruitModeTiles;
        }
        else
        {
            gameState.gameMode = GameState.Mode.freeMode;
            MapModes mapModes = FindObjectOfType<MapModes>();
            mapModes.mapMode = MapModes.Mode.terrain;
        }
    }

    public void ButtonDeleteArmy()
    {
        CloseArmyUI();
        gameState.activeArmy.DeleteArmy();
    }
}