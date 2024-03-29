using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArmyProps : MonoBehaviour
{
    public Sprite[] sprites;

    public NationProps nation;
    public List<TileProps> reinforceTiles = new List<TileProps>();

    public TMP_Text armySizeText;

    public bool reinforce;
    public bool isInBattle; // this is unused for now 

    public int maxSize;
    public int curSize;
    public int availablePop;

    public int maxInfantry;
    public int maxCavalry;
    
    public int curInfantry;
    public int curCavalry;

    public float morale;
    public float speed;

    private void Start()
    {
        reinforce = true;
        //isInBattle = false; // FOR SOME FUCKING REASON THIS SHIT WILL FUCK UP NATIVE AMBUSHERS

        speed = 1;
        morale = 10;
    }

    public List<TileProps> GetNeighbors() //I might just get the neighbors of the parent tile instead of doing this bullshit
    {
        List<Vector2> Coords = new List<Vector2> //get coords of neighbors
        {
            new Vector2(transform.position.x + 1, transform.position.y),
            new Vector2(transform.position.x - 1, transform.position.y),
            new Vector2(transform.position.x + 0.5f, transform.position.y + 0.86f),
            new Vector2(transform.position.x - 0.5f, transform.position.y + 0.86f),
            new Vector2(transform.position.x + 0.5f, transform.position.y - 0.86f),
            new Vector2(transform.position.x - 0.5f, transform.position.y - 0.86f)
        };

        List<TileProps> neighbors = new List<TileProps>(); //list for tiles
        
        int tileLayer = LayerMask.GetMask("Tiles"); //HOLY FUCK, FUCK UNITY FUCK LAYERS FUCK EVERYTHING
        foreach (Vector2 neighborCoord in Coords) //get tiles from coords
        {
            
            RaycastHit2D hit = Physics2D.Raycast(neighborCoord, Vector2.zero, 0, tileLayer);
            if (hit.collider != null)
            {
                TileProps neighborTile = hit.collider.GetComponent<TileProps>();
                neighbors.Add(neighborTile);
            }
        }
        return neighbors;
    }

    public void SwitchSprite(int i)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[i];
    }
    
    //I should probably write a method for reinforcements too 

    public void GetReinforced(int i) //placeholder
    {
        int infReinforcement = Mathf.Min(i, maxInfantry - curInfantry);
        int cavReinforcement = Mathf.Min(i, maxCavalry - curCavalry);

        curInfantry += infReinforcement;
        curCavalry += cavReinforcement;
        
        curSize = curInfantry + curCavalry;
    }

    public void TakeLosses(int i) //placeholder
    {
        int infLosses = Mathf.Min(i, curInfantry);
        int cavLosses = Mathf.Min(i, curCavalry);

        curCavalry -= infLosses;
        curInfantry -= cavLosses;
        
        curSize = curInfantry + curCavalry;
    }

    public void DeleteArmy()
    {
        GameState gameState = FindObjectOfType<GameState>();
        UpdateManager updateManager = FindObjectOfType<UpdateManager>();
        ArmyTracker armyTracker = FindObjectOfType<ArmyTracker>();
        
        if (gameState.activeArmy == this)
        {
            gameState.activeArmy = null;
        }

        updateManager.armies.Remove(this); //This is a bit weird. But I don't want to reference update manager in this script directly so this works for now.

        if(isInBattle)
        {
            foreach (KeyValuePair<BattleProps, Vector2> battle in armyTracker.battlePositions) //Checking every battle like this is bad. Find a way of directly referencing the battle this army is in.
            {
                if(battle.Key.attackerArmies.Contains(this))
                {
                    battle.Key.attackerArmies.Remove(this);
                }
                if (battle.Key.defenderArmies.Contains(this))
                {
                    battle.Key.defenderArmies.Remove(this);
                }
            }
        }

        armyTracker.RemoveArmy(this);

        foreach (var tile in reinforceTiles)
        {
            tile.isReinforceTile = false;
        }

        nation.armies.Remove(this);
        Destroy(gameObject);
    }
}
