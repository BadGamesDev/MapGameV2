using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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

    public float agriPop;
    public float resourcePop;
    public float industryPop;
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

    private void Awake()
    {
        infrastructureLevel = 0; //kinda bad
        
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

    public void SetPopulationRatios(float agriRatio, float resourceRatio, float industryRatio)
    {
        float totalRatio = agriRatio + resourceRatio + industryRatio;

        agriPop = totalPop * (agriRatio / totalRatio);
        resourcePop = totalPop * (resourceRatio / totalRatio);
        industryPop = totalPop * (industryRatio / totalRatio);
    }

    public void IncreasePopulation(float percentageIncrease)
    {
        agriPop += agriPop * percentageIncrease;
        resourcePop += resourcePop * percentageIncrease;
        industryPop += industryPop * percentageIncrease;

        totalPop = agriPop + resourcePop + industryPop;
    }

    public int GetTotalPopulation()
    {
        return Mathf.RoundToInt(totalPop);
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
