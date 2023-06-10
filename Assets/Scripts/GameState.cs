using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public TileProps activeTile;
    public UnitProps activeUnit;
    public bool recruitModeArmy;
    public bool recruitModeTiles;

    private void Start()
    {
        recruitModeArmy = false;
        recruitModeTiles = false;
    }
}
