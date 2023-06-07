using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Dictionary<string, float> resourcePrices;
    public List<Resource> resources;

    private void Start()
    {
        InitializeResourcePrices();
        InitializeResources();
    }

    private void InitializeResourcePrices()
    {
        resourcePrices = new Dictionary<string, float>
        {
            { "Grain", 1.0f },
            { "Timber", 1.5f },
            { "Iron", 2.5f },
            { "Coal", 5.0f },
            { "Gold", 10.0f },
        };
    }

    private void InitializeResources()
    {
        resources = new List<Resource>();
        foreach (KeyValuePair<string, float> resource in resourcePrices)
        {
            Resource res = new Resource { Name = resource.Key, Price = resource.Value };
            resources.Add(res);
        }
    }
}