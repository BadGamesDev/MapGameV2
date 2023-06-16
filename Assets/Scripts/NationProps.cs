using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NationProps : MonoBehaviour
{
    public List<TileProps> tiles;
    public List<ArmyProps> armies;
    
    public string nationName;
    public int population;

    public float agriGDP;
    public float resourceGDP;
    public float industryGDP;
    public float totalGDP;

    public float income;
    public float expense;

    public int money;
    public int debt;

    public List<int> populationHistory = new List<int>();

    public Dictionary<string, float> govBuy;
    public Dictionary<string, float> govSell;

    public Dictionary<string, float> supply;
    public Dictionary<string, float> demand;

    public NationProps()
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
}