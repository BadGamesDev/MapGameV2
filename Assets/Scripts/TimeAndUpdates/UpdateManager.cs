using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TimeManager;
using static TileInteractions;
using static NationAI; //THIS MIGHT BE REAAAAALY BAD!!!!!! FIND ANOTHER WAY OF ADDING ARMIES TO THE LIST

public class UpdateManager : MonoBehaviour
{
    public List<ArmyProps> armies;
    public TileProps[] tiles;
    public NationProps[] nations;

    public MainUI mainUI; //for debugging
    public TileUI tileUI;
    public MapGenerator mapGenerator;
    public ResourceManager resourceManager;

    private void Start()
    {
        armies = new List<ArmyProps>();
        tiles = FindObjectsOfType<TileProps>();
        nations = FindObjectsOfType<NationProps>();

        ArmyRecruited += OnArmyRecruited;
        ArmyRecruitedAI += OnArmyRecruited;
        dayTickSend += OnDayTick;
        monthTickSend += OnMonthTick;

        SetInitialRecruits();
    }

    public void SetInitialRecruits()
    {
        foreach (TileProps province in tiles)
        {
            province.recruitPop = Mathf.RoundToInt(province.totalPop / 10);
        }
    }

    private void OnArmyRecruited(ArmyProps newArmy)
    {
        armies.Add(newArmy);
    }

    public void OnDayTick()
    {
        CalculateNationalDemand();
        CalculateNationalSupply();
        CalculateGlobalDemand();
        CalculateGlobalSupply();
        UpdateArmyProps();
        UpdateTileProps();
        UpdateNationProps();
        UpdateTileUI();
    }

    public void OnMonthTick()
    {
        UpdateGraphData();
        AutoNationExpansion();
    }

    public void CalculateNationalDemand() //setting everything to 0 seems to be working for now
    {
        foreach (NationProps nation in nations)
        {
            foreach (var key in nation.demand.Keys.ToList())
            {
                nation.demand[key] = 0;
            }

            foreach (TileProps tile in nation.tiles)
            {
                nation.AddDemand("Grain", tile.totalPop * 0.01f);
            }

            foreach (ArmyProps army in nation.armies) //maybe move the requirements inside the army props script?
            {
                nation.AddDemand("Iron", army.curInfantry * 0.1f);
                nation.AddDemand("Iron", army.curCavalry * 0.3f); nation.AddDemand("Grain", army.curCavalry * 0.3f);
            }
        }
    }

    public void CalculateNationalSupply()
    {
        foreach (NationProps nation in nations)
        {
            foreach (var key in nation.supply.Keys.ToList())
            {
                nation.supply[key] = 0;
            }

            foreach (TileProps tile in nation.tiles)
            {
                nation.AddSupply(tile.agriResource, tile.agriProduction);
                nation.AddSupply(tile.resource, tile.resourceProduction);
            }
        }
    }

    public void CalculateGlobalDemand()
    {
        foreach (string resourceName in resourceManager.globalDemand.Keys.ToList())
        {
            resourceManager.globalDemand[resourceName] = 0;
        }

        foreach (NationProps nation in nations)
        {
            foreach (KeyValuePair<string, float> demandItem in nation.demand)
            {
                string resourceName = demandItem.Key;
                float amount = demandItem.Value;

                if (resourceManager.globalDemand.ContainsKey(resourceName))
                {
                    resourceManager.globalDemand[resourceName] += amount;
                }
                else
                {
                    resourceManager.globalDemand.Add(resourceName, amount);
                }
            }
        }
    }

    public void CalculateGlobalSupply()
    {
        foreach (string resourceName in resourceManager.globalSupply.Keys.ToList())
        {
            resourceManager.globalSupply[resourceName] = 0;
        }

        foreach (NationProps nation in nations)
        {
            foreach (KeyValuePair<string, float> supplyItem in nation.supply)
            {
                string resourceName = supplyItem.Key;
                float amount = supplyItem.Value;

                if (resourceManager.globalSupply.ContainsKey(resourceName))
                {
                    resourceManager.globalSupply[resourceName] += amount;
                }
                else
                {
                    resourceManager.globalSupply.Add(resourceName, amount);
                }
            }
        }
    }

    public void UpdateArmyProps()
    {
        foreach (ArmyProps army in armies)
        {
            army.availablePop = Mathf.RoundToInt(army.reinforceTiles.Sum(tile => tile.recruitPop));//get recruit pops

            if (army.curSize < army.desiredSize)
            {
                int totalPop = Mathf.RoundToInt(army.reinforceTiles.Sum(tile => tile.totalPop)); //total pop of tiles (used for reinforce speed)
                float reinforcements = totalPop * 0.001f;

                reinforcements = Mathf.Min(reinforcements, army.desiredSize - army.curSize, army.availablePop);//is there a problem here?
                int reinforcementsInt = Mathf.RoundToInt(reinforcements); //reinforcement number int

                float infantryRatio = army.maxInfantry / (float)(army.maxInfantry + army.maxCavalry); //get the ratio of diffent troop types
                float cavalryRatio = army.maxCavalry / (float)(army.maxInfantry + army.maxCavalry);

                int infantryToAdd = Mathf.RoundToInt(reinforcementsInt * infantryRatio); //calculate reinforcements based on ratio
                int cavalryToAdd = Mathf.RoundToInt(reinforcementsInt * cavalryRatio);

                int capInfantryToAdd = Mathf.Min(infantryToAdd, army.maxInfantry - army.curInfantry);
                int capCavalryToAdd = Mathf.Min(cavalryToAdd, army.maxCavalry - army.curCavalry);

                int adjustedInfantryToAdd = capInfantryToAdd;
                int adjustedCavalryToAdd = capCavalryToAdd;

                float requiredIron = capInfantryToAdd * 1 + capCavalryToAdd * 3;
                float availableIron = resourceManager.globalSupply["Iron"] - army.nation.demand["Iron"]; //SEPARATE THE NATION AND WORLD SUPPLY IN THE FUTURE

                if (availableIron <= requiredIron) //MIGHT CAUSE SOME ERRORS DUE TO ROUNDING !!! (adjusting reinforcements according to available resources)
                {
                    float ironRatio = (float)availableIron / requiredIron;
                    adjustedInfantryToAdd = Mathf.RoundToInt(capInfantryToAdd * ironRatio);
                    adjustedCavalryToAdd = Mathf.RoundToInt(capCavalryToAdd * ironRatio);

                    army.nation.GovBuy("Iron", availableIron);
                }
                else
                {
                    army.nation.GovBuy("Iron", requiredIron);
                }
                army.curInfantry += adjustedInfantryToAdd;
                army.curCavalry += adjustedCavalryToAdd;



                int finalReinforcement = adjustedInfantryToAdd + adjustedCavalryToAdd;

                foreach (TileProps tile in army.reinforceTiles) //get how much reinforcement each tile gives to substract them
                {
                    int totalReinforcePop = Mathf.RoundToInt(army.reinforceTiles.Sum(tile => tile.recruitPop));

                    float tileRatio = tile.recruitPop / totalReinforcePop;

                    float tileReinforcements = finalReinforcement * tileRatio;
                    tile.recruitPop -= Mathf.RoundToInt(tileReinforcements); //There might be a difference between total population coming from tiles and the reinfrocementsInt. Maybe change some stuff?
                } //here is an idea : add what is substracted from every tile to get a final number?

                army.curSize = army.curInfantry + army.curCavalry;
                army.availablePop -= finalReinforcement;
            }
        }
    }

    public void UpdateTileProps()
    {
        foreach (TileProps tile in mapGenerator.landTilesList)
        {
            //POPULATION ###################################################################################
            tile.IncreasePopulation(0.001f);

            if (tile.recruitPop < tile.totalPop)
            {
                tile.recruitPop += Mathf.RoundToInt(tile.totalPop * 0.005f);
            }

            //ECONOMY ######################################################################################
            tile.agriProduction = tile.GetAgriPopulation() * 0.01f * tile.infrastructureLevel;
            tile.resourceProduction = tile.GetResourcePopulation() * 0.01f * tile.infrastructureLevel;
            tile.industryProduction = tile.GetIndustryPopulation() * 0.01f * tile.infrastructureLevel;

            float globalAgriSupplyAmount = resourceManager.globalSupply[tile.agriResource];//agri
            float globalAgriDemandAmount = resourceManager.globalDemand[tile.agriResource];

            float agriDemandRatio = 1.0f;
            if (globalAgriSupplyAmount > globalAgriDemandAmount)
            {
                agriDemandRatio = globalAgriDemandAmount / globalAgriSupplyAmount;
            }
            tile.agriGDP = resourceManager.resourcePrices[tile.agriResource] * (tile.agriProduction * agriDemandRatio);

            float globalResourceSupplyAmount = resourceManager.globalSupply[tile.resource];//resource
            float globalResourceDemandAmount = resourceManager.globalDemand[tile.resource];

            float resourceDemandRatio = 1.0f;

            if (globalResourceSupplyAmount > globalResourceDemandAmount)
            {
                resourceDemandRatio = globalResourceDemandAmount / globalResourceSupplyAmount;
            }

            tile.resourceGDP = resourceManager.resourcePrices[tile.resource] * (tile.resourceProduction * resourceDemandRatio);

            tile.totalGDP = tile.agriGDP + tile.resourceGDP + tile.industryGDP; //total

            tile.totalPerCapGDP = tile.agriGDP / tile.GetTotalPopulation();
            tile.agriPerCapGDP = tile.resourceGDP / tile.GetAgriPopulation();
            tile.resourcePerCapGDP = tile.industryGDP / tile.GetResourcePopulation();
            //tile.industryPerCapGDP = tile.agriGDP / tile.GetIndustryPopulation();

            //DEVELOPMENT ###################################################################################
            if (tile.nation != null) //THIS SHIT IS BAD FOR PERFORMANCE, HAVING A LIST OF OWNED TILES WOULD FIX A LOT OF STUFF
            {
                tile.infrastructureLevel += 0.002f + 0.00015f * tile.nation.developmentBudget - 0.00050f * tile.infrastructureLevel; //Really bad formula but who cares?
            }
        }
    }

    public void UpdateProvInfrasturcture()
    {
        foreach (TileProps tile in mapGenerator.landTilesList) //MAKE THIS AN OWNED TILES ONLY LIST IN THE FUTURE
        {
             //Will add more things to this formula
        }
    }

    public void UpdateNationProps()
    {
        //calculate Pop
        foreach (NationProps nation in nations)
        {
            int nationPopulation = 0;

            foreach (TileProps province in nation.tiles)
            {
                nationPopulation += province.GetTotalPopulation();
            }

            nation.population = nationPopulation;
        }

        //calculate GDP
        foreach (NationProps nation in nations)
        {
            float agriGDP = 0;
            float resourceGDP = 0;
            float industryGDP = 0;
            float totalGDP = 0;

            foreach (TileProps tile in nation.tiles)
            {
                agriGDP += tile.agriGDP;
                resourceGDP += tile.resourceGDP;
                industryGDP += tile.industryGDP;
                totalGDP += tile.totalGDP;
            }

            nation.agriGDP = agriGDP;
            nation.resourceGDP = resourceGDP;
            nation.industryGDP = industryGDP;
            nation.totalGDP = totalGDP;
        }

        //calculate income
        foreach (NationProps nation in nations)
        {
            float nationTax = 0;
            float nationTaxLevel = nation.taxLevel;

            foreach (TileProps tile in nation.tiles)
            {
                tile.tax = (tile.agriGDP + tile.resourceGDP) * (nationTaxLevel / 100); //Industry missing for now

                nationTax += tile.tax;
            }
            nation.income = nationTax;
        }

        //calculate expense
        foreach (NationProps nation in nations)
        {
            float nationBuy = 0;

            foreach (var kvp in nation.govBuy)
            {
                string resource = kvp.Key;
                float quantity = kvp.Value;

                if (resourceManager.resourcePrices.TryGetValue(resource, out float price)) //SUS
                {
                    nationBuy += price * quantity;
                }
                else
                {
                    Debug.LogError("Resource price not found: " + resource);
                }
            }

            float nationWages = 0;
            foreach (ArmyProps army in nation.armies)
            {
                nationWages += army.curInfantry * 0.1f
                + army.curCavalry * 0.15f;
            }

            float nationInterest = 0;
            if (nation.debt > 0)
            {
                nationInterest = Mathf.RoundToInt(nation.debt * 0.004f);
            }

            nation.expense = nationBuy + nationWages + nationInterest;

            foreach (var key in nation.govBuy.Keys.ToList()) //FEELS WRONG
            {
                nation.govBuy[key] = 0;
            }
        }

        //calculate balance
        foreach (NationProps nation in nations)
        {
            float nationBalance = nation.income - nation.expense;

            nation.money += Mathf.RoundToInt(nationBalance);
        }

        //calculate debt
        foreach (NationProps nation in nations)
        {
            if (nation.money < 0)
            {
                nation.debt += nation.money * -1;
                nation.money = 0;
            }
            else if (nation.money > 0 && nation.debt > 0)
            {
                int i = Mathf.Min(nation.money, nation.debt);

                nation.debt += i * -1;
                nation.money += i * -1;
            }
        }
    }

    public void UpdateBattles()//THIS MIGHT BE AN ATROCIOUS WAY OF IMPLEMENTING COMBAT...
    {
        foreach (TileProps tile in tiles)
        {
            //tile.GetComponent<Collider2D>
        }
    }

    public void AutoNationExpansion()
    {
        foreach (NationProps nation in nations)
        {
            List<TileProps> borderTiles = new List<TileProps>(); //get bordertiles
            foreach (TileProps tile in nation.tiles)
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

            if (unclaimedNeighbors.Count > 0) //choose expansion target
            {
                TileProps randomNeighbor = unclaimedNeighbors[Random.Range(0, unclaimedNeighbors.Count)];
                randomNeighbor.nation = nation;
                nation.tiles.Add(randomNeighbor);
            }

            bool IsTileOwnedByNation(Vector2 position) //method used to see if a tile is border
            {
                RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
                if (hit.collider != null)
                {
                    TileProps tile = hit.collider.GetComponent<TileProps>();
                    if (tile != null && tile.nation == nation)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }

    public void UpdateGraphData()
    {
        foreach (NationProps nation in nations)
        {
            nation.populationHistory.Add(nation.population);

            if (nation.populationHistory.Count > 30)
            {
                nation.populationHistory.RemoveAt(0);
            }
        }
    }

    public void UpdateTileUI()
    {
        if (tileUI.panelUI.activeSelf == true)
        { 
            tileUI.UpdateTileUI();
        }
    }
}
