using System.Collections.Generic;
using UnityEngine;

public class TileProps : MonoBehaviour
{
    public Sprite[] sprites;

    public GameObject FOW;
    public SpriteRenderer resourceSprite; //maybe make it an array?
    public NationProps nation = null;

    public List<Vector2> neighbors;

    public int ID;
    
    public int type;

    public int grow = 1;
    public int freq = 1;

    public bool isReinforceTile;
    public int recruitPop;

    public float popGrowthRateNonTribal;

    public float tribalPop;
    public float agriPop;
    public float resourcePop;
    public float industryPop;
    public float totalPopNonTribal;
    public float totalPop;
    
    public string resource;
    public string agriResource;

    public float agriProduction;
    public float resourceProduction;
    public float industryProduction;

    public float agriGDP;
    public float resourceGDP;
    public float industryGDP;
    public float totalGDP;

    public float agriPerCapGDP;
    public float resourcePerCapGDP;
    public float industryPerCapGDP;
    public float totalPerCapGDP;

    public float infrastructureLevel; //SUBJECT TO CHANGE
    public float devCost;

    public float tax; //might change

    public float attraction;
    public float attractionModifier; //I'm sure there can be a better way
    public float migration; //mostly for debugging

    public int nativeAgressiveness;

    private void Awake()
    {
        infrastructureLevel = 0; //kinda bad
        attraction = 0;
        
        isReinforceTile = false;

        neighbors = new List<Vector2>
        {
            new Vector2(transform.position.x + 1, transform.position.y),
            new Vector2(transform.position.x - 1, transform.position.y),
            new Vector2(transform.position.x + 0.5f, transform.position.y + 0.86f),
            new Vector2(transform.position.x - 0.5f, transform.position.y + 0.86f),
            new Vector2(transform.position.x + 0.5f, transform.position.y - 0.86f),
            new Vector2(transform.position.x - 0.5f, transform.position.y - 0.86f)
        };
    }

    public void SetPopulationRatios(float tribalRatio, float agriRatio, float resourceRatio, float industryRatio)
    {
        float totalRatio = tribalRatio + agriRatio + resourceRatio + industryRatio;

        tribalPop = totalPop * (tribalRatio / totalRatio);
        agriPop = totalPop * (agriRatio / totalRatio);
        resourcePop = totalPop * (resourceRatio / totalRatio);
        industryPop = totalPop * (industryRatio / totalRatio);
    }

    public void IncreaseAllPopsPercent(float percentIncrease) //Increases or decreases don't update UI by themselves which is a bit of a problem. These are all non tribal changes btw
    {
        agriPop += agriPop * percentIncrease;
        resourcePop += resourcePop * percentIncrease;
        industryPop += industryPop * percentIncrease;

        totalPop = tribalPop + agriPop + resourcePop + industryPop;
        totalPopNonTribal = agriPop + resourcePop + industryPop;
    }

    public void DecreaseAllPopsPercent(float percentDecrease)
    {
        agriPop -= agriPop * percentDecrease;
        resourcePop -= resourcePop * percentDecrease;
        industryPop -= industryPop * percentDecrease;

        totalPop = tribalPop + agriPop + resourcePop + industryPop;
        totalPopNonTribal = agriPop + resourcePop + industryPop;
    }

    public void IncreaseAllPopsFlat(float flatIncrease)
    {
        float agriFlatIncrease = agriPop / totalPopNonTribal * flatIncrease;
        float resourceFlatIncrease = resourcePop / totalPopNonTribal * flatIncrease;
        float industryFlatIncrease = industryPop / totalPopNonTribal * flatIncrease;

        agriPop += agriFlatIncrease;
        resourcePop += resourceFlatIncrease;
        industryPop += industryFlatIncrease;

        totalPop = tribalPop + agriPop + resourcePop + industryPop;
        totalPopNonTribal = agriPop + resourcePop + industryPop;
    }

    public void DecreaseAllPopsFlat(float flatDecrease)
    {
        float agriFlatDecrease = agriPop / totalPopNonTribal * flatDecrease;
        float resourceFlatDecrease = resourcePop / totalPopNonTribal * flatDecrease;
        float industryFlatDecrease = industryPop / totalPopNonTribal * flatDecrease;

        agriPop -= agriFlatDecrease;
        resourcePop-= resourceFlatDecrease;
        industryPop -= industryFlatDecrease;

        totalPop = tribalPop + agriPop + resourcePop + industryPop;
        totalPopNonTribal = agriPop + resourcePop + industryPop;
    }

    public int GetTotalPopulation()
    {
        return Mathf.RoundToInt(GetTribalPopulation() + GetAgriPopulation() + GetResourcePopulation() + GetIndustryPopulation());
    }

    public int GetTotalPopulationNonTribal()
    {
        return Mathf.RoundToInt(GetAgriPopulation() + GetResourcePopulation() + GetIndustryPopulation());
    }

    public int GetTribalPopulation()
    {
        return Mathf.RoundToInt(tribalPop);
    }

    public int GetAgriPopulation()
    {
        return Mathf.RoundToInt(agriPop);
    }

    public int GetResourcePopulation()
    {
        return Mathf.RoundToInt(resourcePop);
    }

    public int GetIndustryPopulation()
    {
        return Mathf.RoundToInt(industryPop);
    }

    public void SwitchType(int i)
    {
        type = i;
    }
 
    public void SwitchSprite(int i)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[i];
    }
    
    public void Grow() //for generating land masses on map
    {
        for (int i = 0; i < freq; i++)
        {
            int n = Mathf.RoundToInt(Random.value * 5);
            
            RaycastHit2D hit = Physics2D.Raycast(neighbors[n], neighbors[n], 0, LayerMask.GetMask("Tiles"));
            if (hit)
            {
                TileProps newTile = hit.collider.gameObject.GetComponent<TileProps>();

                if (newTile.type == 1)
                {
                    newTile.SwitchType(2);

                    if (grow - 1 > 0)
                    { 
                        newTile.grow = grow - 1;
                        newTile.freq = freq;

                        newTile.Grow();
                    }
                }
            }
        }
    }
}
