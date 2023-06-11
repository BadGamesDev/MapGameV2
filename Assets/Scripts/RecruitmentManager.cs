using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecruitmentManager : MonoBehaviour
{
    public GameState gameState;

    public TMP_InputField desiredSizeInput;

    public void RecruitArmy()
    {
        gameState.recruitModeArmy = true;

        desiredSizeInput.gameObject.SetActive(true);
        desiredSizeInput.onEndEdit.AddListener(OnArmySizeEntered);
    }

    private void OnArmySizeEntered(string input)
    {
        int size = int.Parse(input);
        gameState.activeUnit.desiredSize = size;

        desiredSizeInput.gameObject.SetActive(false);

        desiredSizeInput.onEndEdit.RemoveListener(OnArmySizeEntered);
    }
}