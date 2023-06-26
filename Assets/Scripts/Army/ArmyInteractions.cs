using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (gameState.gameMode == GameState.Mode.freeMode)
            {
                gameState.activeArmy = armyProps;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
