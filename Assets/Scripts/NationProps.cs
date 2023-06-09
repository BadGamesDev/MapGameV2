using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NationProps : MonoBehaviour
{
    public string nationName;
    public List<TileProps> tiles;
    public int population;

    public float agriGDP;
    public float resourceGDP;
    public float industryGDP;
    public float totalGDP;

    public int money;
    public int debt;

    public List<int> populationHistory = new List<int>();

    public Dictionary<string, float> supply;
    public Dictionary<string, float> demand;


    public NationProps()
    {
        supply = new Dictionary<string, float>();
        demand = new Dictionary<string, float>();
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