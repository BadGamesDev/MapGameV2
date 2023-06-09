using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TimeManager;
using static TileInteractions;
using static NationAI; //THIS MIGHT BE REAAAAALY BAD!!!!!! FIND ANOTHER WAY OF ADDING ARMIES TO THE LIST

public class UpdateManager : MonoBehaviour
{
    public GameState gameState;
    public MapGenerator mapGenerator;
    public ResourceManager resourceManager;
    public ArmyTracker armyTracker;
    public CamControl camControl;
    
    public List<ArmyProps> armies;
    public TileProps[] tiles;
    public NationProps[] nations; //tiles can be an array but this should probably be a list
    
    public MainUI mainUI; //for debugging
    public TileUI tileUI;
    public ArmyUI armyUI;
    public EconomyUI economyUI;
    public TradeUI tradeUI;

    private void Start()
    {
        armies = new List<ArmyProps>();
        tiles = FindObjectsOfType<TileProps>();
        nations = FindObjectsOfType<NationProps>();

        ArmyRecruited += OnArmyRecruited;
        ArmyRecruitedAI += OnArmyRecruited;
        dayTickSend += OnDayTick;
        monthTickSend += OnMonthTick;

        SetupResources();
        SetInitialRecruits();

        //first tick
        CalculateNationalDemand();
        CalculateNationalSupply();
        CalculateGlobalDemand();
        CalculateGlobalSupply();
        UpdateArmyProps();
        UpdateTileProps();
        UpdateNationProps();
        UpdateMainUI();
        UpdateTileUI();
        UpdateArmyUI();
        UpdateEconomyUI();
        UpdateTradeUI();

        camControl.CenterCamera(gameState.playerNation.capital.gameObject);
    }

    public void SetupResources()
    {
        resourceManager.InitializeResourcePrices();
        resourceManager.InitializeResources();
        resourceManager.InitializeSupplyDemand();
    }
    
    public void SetInitialRecruits() //not "bad" but there is definitely a cleaner way
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
        MoveArmies();
        UpdateBattles();
        UpdateMainUI();
        UpdateTileUI();
        UpdateArmyUI();
        UpdateEconomyUI();
        UpdateTradeUI();
    }

    public void OnMonthTick()
    {
        UpdateGraphData();
        //AutoNationExpansion();
    }

    public void CalculateNationalDemand() //setting everything to 0 seems to be working for now
    {
        foreach (NationProps nation in nations)
        {
            foreach (var key in nation.demand.Keys.ToList())
            {
                nation.demand[key] = 0;
            }

            foreach (TileProps tile in nation.ownedTiles)
            {
                nation.AddDemand("Grain", tile.totalPop * 0.004f);
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

            foreach (TileProps tile in nation.ownedTiles)
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

    public void UpdateTileProps()
    {
        foreach (TileProps tile in mapGenerator.landTiles)
        {
            //POPULATION ###################################################################################
            tile.IncreasePopulation(0.0001f);

            if (tile.recruitPop < tile.totalPop / 10)
            {
                tile.recruitPop += Mathf.RoundToInt(tile.totalPop * 0.001f);
            }

            //ECONOMY ######################################################################################
            tile.agriProduction = tile.GetAgriPopulation() * 0.01f * (tile.infrastructureLevel / 100 + 0.20f); //placeholder formula, also I feel like development could be simplified so that I don't have to divide it by 100.
            tile.resourceProduction = tile.GetResourcePopulation() * 0.01f * (tile.infrastructureLevel / 100 + 0.20f);
            tile.industryProduction = tile.GetIndustryPopulation() * 0.01f * (tile.infrastructureLevel / 100 + 0.20f);

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
                tile.infrastructureLevel += 0.002f + 0.015f * tile.nation.developmentBudget - 0.0005f * tile.infrastructureLevel; //Really bad formula but who cares?
            }
        }
    }

    public void UpdateNationProps()
    {
        //calculate Pop
        foreach (NationProps nation in nations)
        {
            int nationPopulation = 0;

            foreach (TileProps province in nation.ownedTiles)
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

            foreach (TileProps tile in nation.ownedTiles)
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

            foreach (TileProps tile in nation.ownedTiles) //Maybe move this to tile update?
            {
                tile.tax = (tile.agriGDP + tile.resourceGDP) * (nationTaxLevel); //Industry missing for now

                nationTax += tile.tax;
            }
            nation.income = nationTax; //this is just placeholder, income calculation will not be simple like this
            nation.incomeTax = nationTax;
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

            float nationMilWages = 0;
            foreach (ArmyProps army in nation.armies)
            {
                nationMilWages += army.curInfantry * 0.1f
                + army.curCavalry * 0.15f;
            }

            float nationInterest = 0;
            if (nation.debt > 0)
            {
                nationInterest = nation.debt * 0.004f;
            }

            nation.expense = nationBuy + nationMilWages + nationInterest;

            foreach (var key in nation.govBuy.Keys.ToList()) //FEELS WRONG
            {
                nation.govBuy[key] = 0;
            }
            
            float nationExpenseDev = 0;
            foreach (TileProps tile in nation.ownedTiles) //Maybe move this to tile update?
            {
                float developmentReq = (tile.totalPop / 500) * nation.developmentBudget;
                tile.nation.GovBuy("Timber", developmentReq);
                float developmentCost = resourceManager.resourcePrices["Timber"] * developmentReq;
                nationExpenseDev += developmentCost;
            }

            nation.expenseDev = nationExpenseDev;
          
            int nationExpenseMil = 0;//mil expense is calculated in the UpdateArmyProps. (yeah I know, couldn't do it ny other way). 

            nation.expenseMil = nationExpenseMil;
        }

        //calculate balance
        foreach (NationProps nation in nations)
        {
            float nationBalance = nation.income - nation.expense;

            nation.money += nationBalance;
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
                float i = Mathf.Min(nation.money, nation.debt);

                nation.debt += i * -1;
                nation.money += i * -1;
            }
        }
    }

    public void UpdateArmyProps()
    {
        foreach (ArmyProps army in armies)
        {
            if (army.reinforce == true && army.curSize < army.maxSize && army.isInBattle == false) //Reinforce the army
            {
                army.GetReinforced(10);
            }
        }
    }

    public void UpdateBattles()
    {
       foreach (BattleProps battle in armyTracker.battlePositions.Keys)
       {
            foreach(ArmyProps army in battle.attackerArmies)
            {
                army.TakeLosses(10);
                
                if (army.curSize <= 0) 
                {
                    army.DeleteArmy();
                }
            }
            
            foreach (ArmyProps army in battle.defenderArmies)
            {
                army.TakeLosses(10);

                if (army.curSize <= 0)
                {
                    army.DeleteArmy();
                }
            }

            if(battle.attackerArmies.Count == 0 || battle.defenderArmies.Count == 0)
            {
                battle.EndBattle();
            }
        }
    }

    public void AutoNationExpansion() //THINKING OF REMOVING THIS SHIT
    {
        foreach (NationProps nation in nations)
        {
            if (nation.GetNationEmptyNeighbors().Count > 0 && nation.isAI == true) //choose expansion target
            {
                TileProps randomNeighbor = nation.GetNationEmptyNeighbors()[Random.Range(0, nation.GetNationEmptyNeighbors().Count)];
                randomNeighbor.nation = nation;
                nation.ownedTiles.Add(randomNeighbor);
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

    public void UpdateMainUI()
    {
        mainUI.UpdateMainDisplay();
    }

    public void UpdateTileUI()
    {
        if (tileUI.panelUI.activeSelf == true)
        { 
            tileUI.UpdateTileUI();
        }
    }
    
    public void UpdateArmyUI()
    {
        if(armyUI.panelUI.activeSelf == true)
        {
            if (gameState.activeArmy != null)
            {
                armyUI.UpdateArmyUI();
            }
            else
            {
                armyUI.panelUI.SetActive(false);
            }
        }
    }

    public void UpdateEconomyUI()
    {
        economyUI.taxIncomeText.text = gameState.playerNation.incomeTax.ToString();
        economyUI.devExpenseText.text = gameState.playerNation.expenseDev.ToString();
        economyUI.milExpenseText.text = gameState.playerNation.expenseMil.ToString();
    }

    public void UpdateTradeUI()
    {
        tradeUI.UpdateSupplyDemandDisplay();
    }

    public void MoveArmies() //PLEASE FOR THE LOVE OF GOD OPTIMISE THIS
    {
        foreach (ArmyProps army in armies)
        {
            army.gameObject.GetComponent<ArmyMovement>().MarchArmy();
        }
    }
}
