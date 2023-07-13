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
            if (gameState.playerNation == armyProps.nation) //This is temporary, player should be able to check armies of everyone
            {
                if (!EventSystem.current.IsPointerOverGameObject()) //Apparently this is not a good way of making sure you don't click behind UI but it works for now.
                {
                    if (gameState.gameMode == GameState.Mode.freeMode)
                    {
                        if (gameState.activeArmy != null)
                        {
                            gameState.activeArmy.SwitchSprite(0);
                        }

                        gameState.activeArmy = armyProps;
                        armyProps.SwitchSprite(1);
                        
                        armyUI.OpenArmyUI();
                    }
                }
            }
        }
    }
}
