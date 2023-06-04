using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TileType : MonoBehaviour
{
    public Sprite[] sprites;

    public int type;

    public int grow = 1;
    public int freq = 1;

    public int mapWidth = 40;

    public void SwitchType(int i)
    {
        type = i;
        GetComponent<SpriteRenderer>().sprite = sprites[i];
    }

    public void Grow()
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
                TileType newTile = hit.collider.gameObject.GetComponent<TileType>();

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
