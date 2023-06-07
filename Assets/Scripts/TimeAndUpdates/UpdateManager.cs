using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TimeManager;

public class UpdateManager : MonoBehaviour
{
    public TileProps[] tiles;
    public NationProps[] nations;

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
            province.recruitPop = Mathf.RoundToInt(province.population / 10);
        }
    }

    public void OnDayTick()
    {
        UpdateProvProps();
        UpdateNationProps();
    }

    public void OnMonthTick()
    {
        //PayTroopWages();
        UpdateGraphData();
    }

    public void UpdateGraphData()
    {
        foreach (NationProps nation in nations) //calculation is happening twice. Find a better way
        {
            int nationPopulation = 0;

            foreach (TileProps province in nation.tiles)
            {
                nationPopulation += province.population;
            }

            nation.population = nationPopulation;

            nation.populationHistory.Add(nation.population);

            if (nation.populationHistory.Count > 30)
            {
                nation.populationHistory.RemoveAt(0);
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

        //foreach (NationProps nation in nations) //calculate troops
        //{
        //    int nationTroops = 0;

        //    foreach (TileProps province in nation.tiles)
        //    {
        //        nationTroops += province.troops;
        //    }

        //    nation.troops = nationTroops;
        //}
        foreach (NationProps nation in nations) //calculate production
        {
            foreach (TileProps tile in nation.tiles)
            {
                nation.AddResource(tile.agriResource, tile.agriProduction);
            }
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

    //public void PayTroopWages()
    //{
    //    foreach (NationProps nation in nations) //calculate wages
    //    {
    //        int nationWage = 0;

    //        foreach (TileProps province in nation.tiles)
    //        {
    //            nationWage += province.provTroops * 5;
    //        }

    //        nation.money -= nationWage;
    //    }
    //}
}
