using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeManager;

public class UpdateManager : MonoBehaviour
{
    public TileProps[] tiles;
    public NationProps[] nations;

    public ResourceManager resourceManager;

    public Dictionary<string, float> globalDemand { get; private set; } // MIGHT MOVE TO SOMEWHERE ELSE
    public Dictionary<string, float> globalSupply { get; private set; }

    public UpdateManager()
    {
        globalSupply = new Dictionary<string, float>();
        globalDemand = new Dictionary<string, float>();
    }

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
            province.recruitPop = Mathf.RoundToInt(province.population / 10);
        }
    }

    public void OnDayTick()
    {
        UpdateProvProps();
        CalculateNationalDemand();
        CalculateNationalSupply();
        CalculateGlobalDemand();
        CalculateGlobalSupply();
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
                nation.AddDemand("Grain", tile.population * 0.05f);
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
            }
        }
    }

    public void CalculateGlobalDemand()
    {
        globalDemand.Clear();

        foreach (NationProps nation in nations)
        {
            foreach (KeyValuePair<string, float> demandItem in nation.demand)
            {
                string resourceName = demandItem.Key;
                float amount = demandItem.Value;

                if (globalDemand.ContainsKey(resourceName))
                {
                    globalDemand[resourceName] += amount;
                }
                else
                {
                    globalDemand.Add(resourceName, amount);
                }
            }
        }
    }

    public void CalculateGlobalSupply()
    {
        globalSupply.Clear();

        foreach (NationProps nation in nations)
        {
            foreach (KeyValuePair<string, float> supplyItem in nation.supply)
            {
                string resourceName = supplyItem.Key;
                float amount = supplyItem.Value;

                if (globalSupply.ContainsKey(resourceName))
                {
                    globalSupply[resourceName] += amount;
                }
                else
                {
                    globalSupply.Add(resourceName, amount);
                }
            }
        }
    }

    public void UpdateProvProps()
    {
        foreach (TileProps tile in tiles) //population
        {
            float populationIncrease = tile.population * Random.Range(0.0001f, 0.0005f);
            tile.population += Mathf.RoundToInt(populationIncrease);

            if (tile.recruitPop < tile.population * 0.10)
            {
                float recruitPopIncrease = tile.population * Random.Range(0.0003f, 0.0005f);
                tile.recruitPop += Mathf.RoundToInt(recruitPopIncrease);
            }

            tile.agriProduction = tile.agriPop * 0.01f;
        }
    }

    public void UpdateNationProps()
    {
        foreach (NationProps nation in nations) //calculate Pop
        {
            int nationPopulation = 0;

            foreach (TileProps province in nation.tiles)
            {
                nationPopulation += province.population;
            }

            nation.population = nationPopulation;
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
