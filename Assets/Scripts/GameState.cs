using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public NationProps playerNation;

    public TileProps activeTile;
    public ArmyProps activeArmy; //army and navy should probably be a list because the player needs t be able to select more than one
    public NavyProps activeNavy; 

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

    public void SelectArmy(ArmyProps army)
    {
        activeArmy = army;
        army.SwitchSprite(1);
    }

    public void DeselectArmy(ArmyProps army)
    {
        activeArmy = null;
        army.SwitchSprite(0);
    }

    public void SelectNavy(NavyProps navy)
    {
        activeNavy = navy;
        navy.SwitchSprite(1);
    }

    public void DeselectNavy(NavyProps navy)
    {
        activeNavy = null;
        navy.SwitchSprite(0);
    }

    public void SelectTile(TileProps tile)
    {
        activeTile = tile;
    }

    public void DeselectTile(TileProps tile)
    {
        activeTile = null;
    }

    public void ChoosePlayerNation() //Used in MapGenerator
    {
        playerNation = GameObject.Find("Nation0").GetComponent<NationProps>(); //Hardcoding this is kinda bad, but it is fine for now
        playerNation.isAI = false;
    }
}
