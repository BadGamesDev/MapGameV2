using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecruitmentManager : MonoBehaviour
{
    public GameState gameState;
    public MainUI mainUI;
    public MapModes mapModes; //ATROCIOUS SOLUTION! FIND ANOTHER WAY!


    public void RecruitArmyButton()
    {
        gameState.activeArmy = null;

        mainUI.ArmyDesiredSizeInput.onEndEdit.AddListener(OnArmySizeEntered);

        gameState.gameMode = GameState.Mode.recruitModeArmy;
    }

    private void OnArmySizeEntered(string input)
    {
        int size = int.Parse(input);
        gameState.activeArmy.desiredSize = size;

        mainUI.ArmyDesiredSizeInput.gameObject.SetActive(false);
        mainUI.ArmyDesiredSizeInput.onEndEdit.RemoveListener(OnArmySizeEntered);

        gameState.gameMode = GameState.Mode.freeMode;
        mapModes.mapMode = MapModes.Mode.political;
    }
}