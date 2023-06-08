using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NationProps : MonoBehaviour
{
    public string nationName;
    public List<TileProps> tiles;
    public int population;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Supply:");
            foreach (KeyValuePair<string, float> supplyItem in supply)
            {
                Debug.Log(supplyItem.Key + ": " + supplyItem.Value);
            }

            Debug.Log("Demand:");
            foreach (KeyValuePair<string, float> demandItem in demand)
            {
                Debug.Log(demandItem.Key + ": " + demandItem.Value);
            }

            Debug.Log("Global Supply:");
            foreach (KeyValuePair<string, float> globalSupplyItem in GameObject.FindObjectOfType<UpdateManager>().globalSupply)
            {
                Debug.Log(globalSupplyItem.Key + ": " + globalSupplyItem.Value);
            }

            Debug.Log("Global Demand:");
            foreach (KeyValuePair<string, float> globalDemandItem in GameObject.FindObjectOfType<UpdateManager>().globalDemand)
            {
                Debug.Log(globalDemandItem.Key + ": " + globalDemandItem.Value);
            }
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