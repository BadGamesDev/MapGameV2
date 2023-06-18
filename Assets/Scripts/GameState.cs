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
}
