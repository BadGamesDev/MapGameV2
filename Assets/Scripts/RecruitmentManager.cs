using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.Intrinsics;

public class RecruitmentManager : MonoBehaviour
{
    public GameState gameState;
    public MainUI mainUI;
    public MapModes mapModes; //ATROCIOUS SOLUTION! FIND ANOTHER WAY!


    public void RecruitArmyButton()
    {
        gameState.activeArmy = null;
        gameState.gameMode = GameState.Mode.recruitModeArmy;
    }

    public void AssignTroopValues()
    {
        int infantryCount = int.Parse(mainUI.armyInfantrySizeInput.text);
        int cavalryCount = int.Parse(mainUI.armyCavalrySizeInput.text);

        gameState.activeArmy.maxInfantry = infantryCount;
        gameState.activeArmy.maxCavalry = cavalryCount;
        gameState.activeArmy.desiredSize = infantryCount + cavalryCount;

        mainUI.armyInfantrySizeInput.gameObject.SetActive(false);
        mainUI.armyCavalrySizeInput.gameObject.SetActive(false);
        mainUI.armySizeDoneButton.gameObject.SetActive(false);
        
        mapModes.mapMode = MapModes.Mode.political;
        gameState.gameMode = GameState.Mode.freeMode;
    }
}