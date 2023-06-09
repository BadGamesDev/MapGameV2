using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public List<Resource> resources;
    
    public Dictionary<string, float> resourcePrices;
    public Dictionary<string, float> globalDemand;
    public Dictionary<string, float> globalSupply;

    public class Resource
    {
        public string Name { get; set; }
        public float Price { get; set; }
    }

    public void InitializeResourcePrices()
    {
        resourcePrices = new Dictionary<string, float>
        {
            { "Grain", 1.0f },
            { "Timber", 1.5f },
            { "Iron", 2.5f },
            { "Coal", 4.0f },
            { "Gold", 10.0f },
        };
    }

    public void InitializeResources()
    {
        resources = new List<Resource>();
        foreach (KeyValuePair<string, float> resource in resourcePrices)
        {
            Resource res = new Resource { Name = resource.Key, Price = resource.Value };
            resources.Add(res);
        }
    }

    public void InitializeSupplyDemand()
    {
        globalSupply = new Dictionary<string, float>();
        globalDemand = new Dictionary<string, float>();
        
        foreach (Resource resource in resources)
        {
            globalSupply.Add(resource.Name, 0f);
            globalDemand.Add(resource.Name, 0f);
        }
    }
}