using System;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class RecruitmentManager : MonoBehaviour
{
    public GameState gameState;
    public MainUI mainUI;
    public ArmyUI armyUI;
    public MapModes mapModes; //ATROCIOUS SOLUTION! FIND ANOTHER WAY!

    public GameObject army;

    public static event Action<ArmyProps> ArmyRecruited; //to add it to armies list in update manager

    public void RecruitArmyButton()
    {
        if (gameState.gameMode != GameState.Mode.recruitModeArmy)
        {
            if (gameState.activeArmy != null)// ?
            {
                gameState.DeselectArmy(gameState.activeArmy);
            }

            gameState.gameMode = GameState.Mode.recruitModeArmy;
        }

        else
        {
            mapModes.mapMode = MapModes.Mode.political; //It should probably be back to the map mode you have choosen but come on dude... we can do that later
            gameState.gameMode = GameState.Mode.freeMode;
        }
    }

    public void AssignTroopValues()
    {
        int infantryCount = int.Parse(mainUI.armyInfantrySizeInput.text);
        int cavalryCount = int.Parse(mainUI.armyCavalrySizeInput.text);

        gameState.activeArmy.maxInfantry = infantryCount;
        gameState.activeArmy.maxCavalry = cavalryCount;
        gameState.activeArmy.maxSize = infantryCount + cavalryCount;

        mainUI.armyInfantrySizeInput.gameObject.SetActive(false);
        mainUI.armyCavalrySizeInput.gameObject.SetActive(false);
        mainUI.armySizeDoneButton.gameObject.SetActive(false);
        mainUI.recruitmentCancelButton.gameObject.SetActive(false);

        gameState.DeselectArmy(gameState.activeArmy);

        mapModes.mapMode = MapModes.Mode.political;
        gameState.gameMode = GameState.Mode.freeMode;
    }

    public void CancelRecruitment()
    {
        gameState.activeArmy.DeleteArmy();

        mainUI.armyInfantrySizeInput.gameObject.SetActive(false);
        mainUI.armyCavalrySizeInput.gameObject.SetActive(false);
        mainUI.armySizeDoneButton.gameObject.SetActive(false);
        mainUI.recruitmentCancelButton.gameObject.SetActive(false);


        mapModes.mapMode = MapModes.Mode.political;
        gameState.gameMode = GameState.Mode.freeMode;
    }

    //MORNING WORK :) 

    public void RecruitArmy(TileProps tile)
    {
        Vector3 spawnPosition = tile.transform.position;
        GameObject newArmy = Instantiate(army, spawnPosition, Quaternion.identity);
        ArmyProps armyProps = newArmy.GetComponent<ArmyProps>();

        gameState.activeArmy = armyProps; //I don't want to highlight the army so I did it like this

        tile.nation.armies.Add(armyProps);
        ArmyRecruited.Invoke(armyProps);

        armyProps.nation = tile.nation;
        armyProps.reinforceTiles.Add(tile);

        mainUI.armyInfantrySizeInput.gameObject.SetActive(true); //the whole input UI is placeholder
        mainUI.armyCavalrySizeInput.gameObject.SetActive(true);
        mainUI.armySizeDoneButton.gameObject.SetActive(true);
        mainUI.recruitmentCancelButton.gameObject.SetActive(true);

        tile.isReinforceTile = true;

        gameState.gameMode = GameState.Mode.recruitModeTiles;
    }

    public void AssignRecruitTiles(TileProps tile)
    {
        gameState.activeArmy.reinforceTiles.Add(tile);
        tile.isReinforceTile = true;
    }
}