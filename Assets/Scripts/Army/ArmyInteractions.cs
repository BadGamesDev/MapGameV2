using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmyInteractions : MonoBehaviour
{
    public ArmyProps armyProps;
    public GameState gameState;
    public ArmyUI armyUI;
    
    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
        armyUI = FindObjectOfType<ArmyUI>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (gameState.playerNation == armyProps.nation && gameState.gameMode == GameState.Mode.freeMode && !EventSystem.current.IsPointerOverGameObject()) //This is temporary, player should be able to check armies of everyone
            {
                if (gameState.activeArmy != null)
                {
                    gameState.DeselectArmy(gameState.activeArmy);
                }

                gameState.SelectArmy(armyProps);
                        
                armyUI.OpenArmyUI();
            }
        }
    }
}
