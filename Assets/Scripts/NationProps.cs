using System.Collections.Generic;
using UnityEngine;

public class NationProps : MonoBehaviour
{
    public bool isAI;

    public List<TileProps> discoveredTiles;
    public List<TileProps> ownedTiles;
    
    public List<ArmyProps> armies;
    
    public string nationName;
    public int population;

    public float agriGDP;
    public float resourceGDP;
    public float industryGDP;
    public float totalGDP;

    public float income;
    public float incomeTax;

    public float expense;
    public float expenseDev;
    public float expenseMil;

    public float taxLevel;

    public float developmentBudget;
    public float militaryBudget;

    public float money;
    public float debt;

    public List<int> populationHistory = new List<int>();

    public Dictionary<string, float> govBuy;
    public Dictionary<string, float> govSell;

    public Dictionary<string, float> supply;
    public Dictionary<string, float> demand;

    public NationProps() //?
    {
        govBuy = new Dictionary<string, float>();
        govSell = new Dictionary<string, float>();

        govBuy["Iron"] = 0;
        govBuy["Grain"] = 0;
        govBuy["Coal"] = 0;
        govBuy["Timber"] = 0;
        govBuy["Gold"] = 0;

        govSell["Iron"] = 0;
        govSell["Grain"] = 0;
        govSell["Coal"] = 0;
        govSell["Timber"] = 0;
        govSell["Gold"] = 0;

        supply = new Dictionary<string, float>();
        demand = new Dictionary<string, float>();

        supply["Iron"] = 0;
        supply["Grain"] = 0;
        supply["Coal"] = 0;
        supply["Timber"] = 0;
        supply["Gold"] = 0;

        demand["Iron"] = 0;
        demand["Grain"] = 0;
        demand["Coal"] = 0;
        demand["Timber"] = 0;
        demand["Gold"] = 0;
    }

    public void GovBuy(string resourceName, float amount)
    {
        if (govBuy.ContainsKey(resourceName))
        {
            govBuy[resourceName] += amount;
        }
        else
        {
            govBuy.Add(resourceName, amount);
        }
    }
    
    public void GovSell(string resourceName, float amount)
    {
        if (govSell.ContainsKey(resourceName))
        {
            govSell[resourceName] += amount;
        }
        else
        {
            govSell.Add(resourceName, amount);
        }
    }

    public void AddSupply(string resourceName, float amount)
    {
        if (supply.ContainsKey(resourceName))
        {
            supply[resourceName] += amount;
        }
        else
        {
            supply.Add(resourceName, amount);
        }
    }
    
    public void AddDemand(string resourceName, float amount)
    {
        if (demand.ContainsKey(resourceName))
        {
            demand[resourceName] += amount;
        }
        else
        {
            demand.Add(resourceName, amount);
        }
    }

    public List<TileProps> GetNationNeighbors()
    {
        List<TileProps> borderTiles = new List<TileProps>(); //get bordertiles

        foreach (TileProps tile in ownedTiles)
        {
            if (tile.neighbors.Exists(neighborPos => !IsTileOwnedByNation(neighborPos)))
            {
                borderTiles.Add(tile);
            }
        }

        List<TileProps> neighbors = new List<TileProps>(); //get neighbors of bordertiles
        foreach (TileProps tile in borderTiles)
        {
            foreach (Vector2 neighborPos in tile.neighbors)
            {
                RaycastHit2D hit = Physics2D.Raycast(neighborPos, Vector2.zero);
                if (hit.collider != null)
                {
                    TileProps neighborTile = hit.collider.GetComponent<TileProps>();
                    
                    if (neighborTile != null) //I feel like I don't need this check
                    {
                        neighbors.Add(neighborTile);
                    }
                }
            }
        }

        bool IsTileOwnedByNation(Vector2 position) //method used to see if a tile is border with the lambda thingy
        {
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
            if (hit.collider != null)
            {
                TileProps tile = hit.collider.GetComponent<TileProps>();
                if (tile != null && tile.nation == this)
                {
                    return true;
                }
            }

            return false;
        }

        return neighbors;
    }

    public List<TileProps> GetNationEmptyNeighbors() // this can be simplified I guess, I already have a get neighbors method
    {
        List<TileProps> borderTiles = new List<TileProps>(); //get bordertiles
        
        foreach (TileProps tile in ownedTiles)
        {
            if (tile.neighbors.Exists(neighborPos => !IsTileOwnedByNation(neighborPos)))
            {
                borderTiles.Add(tile);
            }
        }

        List<TileProps> unclaimedNeighbors = new List<TileProps>(); //get neighbors of bordertiles
        foreach (TileProps tile in borderTiles)
        {
            foreach (Vector2 neighborPos in tile.neighbors)
            {
                RaycastHit2D hit = Physics2D.Raycast(neighborPos, Vector2.zero);
                if (hit.collider != null)
                {
                    TileProps neighborTile = hit.collider.GetComponent<TileProps>();
                    if (neighborTile != null && neighborTile.nation == null && neighborTile.type != 1)
                    {
                        unclaimedNeighbors.Add(neighborTile);
                    }
                }
            }
        }

        bool IsTileOwnedByNation(Vector2 position) //method used to see if a tile is border with the lambda thingy
        {
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
            if (hit.collider != null)
            {
                TileProps tile = hit.collider.GetComponent<TileProps>();
                if (tile != null && tile.nation == this)
                {
                    return true;
                }
            }

            return false;
        }

        return unclaimedNeighbors;
    }
}