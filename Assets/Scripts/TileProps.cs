using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TileProps : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer resourceSprite; //maybe make it an array?
    public GameObject nation = null;

    public int ID;
    
    public int type;

    public int grow = 1;
    public int freq = 1;

    public int mapWidth = 80; //this is fro wrapping in future

    public int population;
    public int agriPop;
    public int recruitPop;
    public int tax; //might change
    public string resource;
    public string oldResource; //this will be removed after map generation is changed
    public string agriResource;

    public float resourceProduction;
    public float agriProduction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with unit");
        UnitProps unit = collision.gameObject.GetComponent<UnitProps>();
        GameObject conqueringNation = unit.nation;
        GetConquered(conqueringNation);
    }

    public void GetConquered(GameObject newNation)
    {
        nation = newNation;
        newNation.GetComponent<NationProps>().tiles.Add(gameObject.GetComponent<TileProps>());
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
        List<Vector2> neighbors = new List<Vector2>();
        neighbors.Add(new Vector2(transform.position.x + 1, transform.position.y));
        neighbors.Add(new Vector2(transform.position.x - 1, transform.position.y));
        neighbors.Add(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.86f));
        neighbors.Add(new Vector2(transform.position.x - 0.5f, transform.position.y + 0.86f));
        neighbors.Add(new Vector2(transform.position.x + 0.5f, transform.position.y - 0.86f));
        neighbors.Add(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.86f));
        
        for (int i = 0; i < freq; i++)
        {
            int n = Mathf.RoundToInt(Random.value * 5);

            if (neighbors[n].x < 0)
            {
                Vector2 newNeighbor = new Vector2(neighbors[n].x + mapWidth, neighbors[n].y);
                neighbors[n] = newNeighbor;
            }
            
            else if (neighbors[n].x >= mapWidth)
            {
                Vector2 newNeighbor = new Vector2(neighbors[n].x - mapWidth, neighbors[n].y);
                neighbors[n] = newNeighbor;
            }
            
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
                        newTile.mapWidth = mapWidth;

                        newTile.Grow();
                    }
                }
            }
        }
    }
}
