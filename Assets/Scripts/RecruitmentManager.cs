using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecruitmentManager : MonoBehaviour
{
    public GameState gameState;
    public MapModes mapModes; //ATROCIOUS SOLUTION! FIND ANOTHER WAY!

    public TMP_InputField desiredSizeInput;

    public void RecruitArmyButton()
    {
        gameState.activeArmy = null;

        desiredSizeInput.gameObject.SetActive(true);
        desiredSizeInput.onEndEdit.AddListener(OnArmySizeEntered);

        gameState.gameMode = GameState.Mode.recruitModeArmy;
    }

    private void OnArmySizeEntered(string input)
    {
        int size = int.Parse(input);
        gameState.activeArmy.desiredSize = size;

        desiredSizeInput.gameObject.SetActive(false);
        desiredSizeInput.onEndEdit.RemoveListener(OnArmySizeEntered);

        gameState.gameMode = GameState.Mode.freeMode;
        mapModes.mapMode = MapModes.Mode.political;
    }
}