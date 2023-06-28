using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmyInteractions : MonoBehaviour
{
    public ArmyProps armyProps;
    public GameState gameState;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) //APPARENTLY THIS IS ACTUALLY BAD BUT I DON'T KNOW WHY
            {
                if (gameState.gameMode == GameState.Mode.freeMode)
                {
                    gameState.activeArmy = armyProps;
                }
            }
        }
    }
}
