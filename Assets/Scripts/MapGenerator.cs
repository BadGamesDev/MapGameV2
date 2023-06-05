using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tile;
    public GameObject tiles; //parent in editor

    public int masses = 15;

    public Vector2 mapSize = new Vector2(40, 20);
    
    public Vector2 grow = new Vector2(4, 7);
    public int freq = 3;
    

    private void Start()
    {
        GenerateMap();
    }
    
    void GenerateMap()
    {
        //Generate baseline map
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector2 pos = new Vector2(x, y * 0.86f);

                if (y % 2 == 0)
                { 
                    pos.x += 0.5f;
                }
                
                tile.GetComponent<TileType>().SwitchType(1); //maybe there is a better way?
                Instantiate(tile, pos, Quaternion.identity, tiles.transform);
            }
        }

        //Fill the map
        int massTemp = masses;
        int attempt = 0;

        while (massTemp > 0 && attempt < masses * 2)
        {
            attempt++;

            Vector2 pos = new Vector2(Mathf.Round(Random.value * mapSize.x -1), Mathf.Round(Random.value * mapSize.y - 1));

            if (pos.y % 2 == 0)
            {
                pos.x += 0.5f;
            }

            pos.y *= 0.86f;

            RaycastHit2D hit = Physics2D.Raycast(pos, pos, 0, LayerMask.GetMask("Default"));
            
            if (hit)
            {
                TileType newTile = hit.collider.gameObject.GetComponent<TileType>();

                if(newTile.type == 1)
                {
                    newTile.SwitchType(2);

                    newTile.grow = Mathf.RoundToInt(grow.x + Random.value * (grow.y - grow.x));
                    newTile.freq = freq;
                    newTile.mapWidth = (int)mapSize.x;

                    newTile.Grow();

                    massTemp -= 1;
                }
            }
        }
        Invoke("DrawGrid", Time.deltaTime);
    }

    void DrawGrid()
    {
        GetComponent<PathGrid>().width = (int)mapSize.x;
        GetComponent<PathGrid>().height = (int)mapSize.y;

        GetComponent<PathGrid>().DrawGrid();
    }
}
