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
        gameState = GameObject.FindObjectOfType<GameState>();
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            if (gameState.recruitModeArmy == true && tileProps.nation != null)
            {
                Vector3 spawnPosition = transform.position;
                Instantiate(Army, spawnPosition, Quaternion.identity);
                
                UnitProps ArmyProps = Army.GetComponent<UnitProps>();
                ArmyProps.nation = tileProps.nation;
                ArmyProps.reinforceTiles.Add(tileProps);

                gameState.recruitModeArmy = false;
                gameState.recruitModeTiles = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetConquered(collision.gameObject.GetComponent<UnitProps>().nation);
        Debug.Log("Clicked Tile: " + name);
    }

    public void GetConquered(GameObject newNation)
    {
        if (newNation != null && newNation != tileProps.nation)
        {
            if (tileProps.nation != null)
            {
                tileProps.nation.GetComponent<NationProps>().tiles.Remove(tileProps);
            }
            newNation.GetComponent<NationProps>().tiles.Add(tileProps);
            tileProps.nation = newNation;
        }
    }
}
