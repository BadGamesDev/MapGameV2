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

    public Dictionary<string, float> resources;

    public NationProps()
    {
        resources = new Dictionary<string, float>
        {
            { "coal", 0f },
            { "gold", 0f },
            { "iron", 0f },
            { "timber", 0f },
            { "grain", 0f },
            { "cotton", 0f }
        };
    }

    public void GainTile(int ID)
    {   
    }

    public void AddResource(string resourceName, float amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] += amount;
        }
        else
        {
            resources.Add(resourceName, amount);
        }
    }
}