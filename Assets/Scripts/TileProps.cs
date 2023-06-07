using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TileProps : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject nation = null;

    public int type;

    public int grow = 1;
    public int freq = 1;

    public int mapWidth = 80; //this is fro wrapping in future

    //MIGHT PUT THIS STUFF IN ANOTHER SCRIPT ##############################################################################
    public int population;
    public int recruitPop;
    public int tax; //might change
    public SpriteRenderer resourceSprite;
    public string resource;
    public string oldResource;
    public string agriResource;
    //#####################################################################################################################

    public void SwitchType(int i)
    {
        type = i;
    }

    public void SwitchSprite(int i)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[i];
    }

    //MIGHT PUT THIS STUFF IN ANOTHER SCRIPT ##############################################################################


    //#####################################################################################################################

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
            
            RaycastHit2D hit = Physics2D.Raycast(neighbors[n], neighbors[n], 0, LayerMask.GetMask("Default"));
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
