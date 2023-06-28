using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public NationProps playerNation;

    public TileProps activeTile;
    public ArmyProps activeArmy;

    public Mode gameMode;

    public enum Mode
    {
        recruitModeArmy,
        recruitModeTiles,
        freeMode
    }

    private void Start()
    {
        gameMode = Mode.freeMode;
    }

    public void ChoosePlayerNation()
    {
        playerNation = GameObject.Find("Nation0").GetComponent<NationProps>(); //Hardcoding this is kinda bad, but it is fine for now
        playerNation.isAI = false;
    }
}
