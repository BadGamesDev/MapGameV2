using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeManager;

public class UpdateManager : MonoBehaviour
{
    public TileProps[] tiles;
    public NationProps[] nations;

    public MapGenerator mapGenerator;
    public ResourceManager resourceManager;

    private void Start()
    {
        tiles = FindObjectsOfType<TileProps>();
        nations = FindObjectsOfType<NationProps>();

        SetInitialRecruits();

        dayTickSend += OnDayTick;
        monthTickSend += OnMonthTick;
    }

    public void SetInitialRecruits()
    {
        foreach (TileProps province in tiles)
        {
            province.recruitPop = Mathf.RoundToInt(province.totalPop / 10);
        }
    }

    public void OnDayTick()
    {
        CalculateNationalDemand();
        CalculateNationalSupply();
        CalculateGlobalDemand();
        CalculateGlobalSupply();
        UpdateProvProps();
        UpdateNationProps();
    }

    public void OnMonthTick()
    {
        UpdateGraphData();
    }

    public void CalculateNationalDemand()
    {
        foreach (NationProps nation in nations)
        {
            nation.demand.Clear();

            foreach (TileProps tile in nation.tiles)
            {
                nation.AddDemand("Grain", tile.totalPop * 0.01f);
            }
        }
    }

    public void CalculateNationalSupply()
    {
        foreach (NationProps nation in nations)
        {
            nation.supply.Clear();

            foreach (TileProps tile in nation.tiles)
            {
                nation.AddSupply(tile.agriResource, tile.agriProduction);
                nation.AddSupply(tile.resource, tile.resourceProduction);
            }
        }
    }

    public void CalculateGlobalDemand()
    {
        //resourceManager.globalDemand.Clear();

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
        //resourceManager.globalSupply.Clear();

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

    public void UpdateProvProps()
    {
        foreach (TileProps tile in mapGenerator.landTilesList) 
        {
            //POPULATION ###################################################################################
            tile.IncreasePopulation(0.01f);
            
            //ECONOMY ######################################################################################
            tile.agriProduction = tile.GetAgriPopulation() * 0.01f;
            tile.resourceProduction = tile.GetResourcePopulation() * 0.01f;
            tile.industryProduction = tile.GetIndustryPopulation() * 0.01f;

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
            //if (globalResourceSupplyAmount > globalResourceDemandAmount)
            //{
            //    resourceDemandRatio = globalResourceDemandAmount / globalResourceSupplyAmount;
            //}
            tile.resourceGDP = resourceManager.resourcePrices[tile.resource] * (tile.resourceProduction * resourceDemandRatio);

            tile.totalGDP = tile.agriGDP + tile.resourceGDP + tile.industryGDP; //total
        }
    }

    public void UpdateNationProps()
    {
        foreach (NationProps nation in nations) //calculate Pop
        {
            int nationPopulation = 0;

            foreach (TileProps province in nation.tiles)
            {
                nationPopulation += province.GetTotalPopulation();
            }

            nation.population = nationPopulation;
        }

        foreach (NationProps nation in nations) //calculate GDP
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


        foreach (NationProps nation in nations) //calculate tax
        {
            int nationTax = 0;

            foreach (TileProps tile in nation.tiles)
            {
                tile.tax = Mathf.RoundToInt((tile.agriProduction * resourceManager.resources.Find(r => r.Name == tile.agriResource).Price) * 0.1f);
                nationTax += tile.tax;
            }
            nation.money += nationTax;
        }

        foreach (NationProps nation in nations) //calculate interest
        {
            if (nation.debt > 0)
            {
                nation.money -= Mathf.RoundToInt(nation.debt * 0.002f);
            }
        }

        foreach (NationProps nation in nations) //calculate debt
        {
            if (nation.money < 0)
            {
                nation.debt += nation.money * -1;
                nation.money = 0;
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
}