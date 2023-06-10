using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractions : MonoBehaviour
{
    public TileProps tileProps;
    public GameState gameState;
    public GameObject Army;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            if (gameState.recruitModeArmy == true && tileProps.nation != null)
            {
                RecruitArmy();
            }
            else if (gameState.recruitModeTiles == true && tileProps.nation != null)
            {
                AssignRecruitTiles();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NationProps collidingNation = collision.gameObject.GetComponent<UnitProps>().nation;
        
        if (collidingNation != null && collidingNation != tileProps.nation)
        { 
            GetConquered(collidingNation); 
        }
    }

    public void RecruitArmy()
    {
        Vector3 spawnPosition = transform.position;
        GameObject newArmy = Instantiate(Army, spawnPosition, Quaternion.identity);

        UnitProps ArmyProps = newArmy.GetComponent<UnitProps>();
        ArmyProps.nation = tileProps.nation;
        ArmyProps.reinforceTiles.Add(tileProps);

        tileProps.isReinforceTile = true;

        gameState.recruitModeArmy = false;
        gameState.recruitModeTiles = true;
    }

    public void AssignRecruitTiles()
    {
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
