using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractions : MonoBehaviour
{
    public TileProps tileProps;
    public GameState gameState;
    public GameObject Army;

    public static event Action<ArmyProps> ArmyRecruited;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            if (gameState.gameMode == GameState.Mode.recruitModeArmy && tileProps.nation != null)
            {
                RecruitArmy();
            }
            else if (gameState.gameMode == GameState.Mode.recruitModeTiles == true && tileProps.nation == gameState.activeArmy.nation)
            {
                AssignRecruitTiles();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NationProps collidingNation = collision.gameObject.GetComponent<ArmyProps>().nation;
        
        if (collidingNation != null && collidingNation != tileProps.nation)
        { 
            GetConquered(collidingNation); 
        }
    }

    public void RecruitArmy()
    {
        Vector3 spawnPosition = transform.position;
        GameObject newArmy = Instantiate(Army, spawnPosition, Quaternion.identity);
        ArmyProps armyProps = newArmy.GetComponent<ArmyProps>();

        gameState.activeArmy = armyProps;
        tileProps.nation.armies.Add(armyProps);
        ArmyRecruited?.Invoke(armyProps);

        armyProps.nation = tileProps.nation;
        armyProps.reinforceTiles.Add(tileProps);

        tileProps.isReinforceTile = true;

        gameState.gameMode = GameState.Mode.recruitModeTiles;
    }

    public void AssignRecruitTiles()
    {
        gameState.activeArmy.reinforceTiles.Add(tileProps);
        tileProps.isReinforceTile = true;
    }

    public void GetConquered(NationProps newNation)
    {
        if (tileProps.nation != null)
        {
            tileProps.nation.GetComponent<NationProps>().tiles.Remove(tileProps);
        }
        newNation.GetComponent<NationProps>().tiles.Add(tileProps);
        tileProps.nation = newNation;
    }
}
