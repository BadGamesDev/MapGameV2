using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject nationPrefab;
    public GameObject tilesParent;

    public int masses = 30;

    public Vector2 mapSize = new Vector2(80, 40);

    public Vector2 grow = new Vector2(4, 7);
    public int freq = 3;

    public int landTiles = 0;

    public List<TileProps> landTilesList;

    private void Awake()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // Generate baseline map
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector2 pos = new Vector2(x, y * 0.86f);

                if (y % 2 == 0)
                {
                    pos.x += 0.5f;
                }

                tilePrefab.GetComponent<TileProps>().SwitchType(1); // maybe there is a better way?
                Instantiate(tilePrefab, pos, Quaternion.identity, tilesParent.transform);
            }
        }

        // Fill the map
        int massTemp = masses;
        int attempt = 0;

        while (massTemp > 0 && attempt < masses * 2)
        {
            attempt++;

            Vector2 pos = new Vector2(Mathf.Round(Random.value * mapSize.x - 1), Mathf.Round(Random.value * mapSize.y - 1));

            if (pos.y % 2 == 0)
            {
                pos.x += 0.5f;
            }

            pos.y *= 0.86f;

            RaycastHit2D hit = Physics2D.Raycast(pos, pos, 0, LayerMask.GetMask("Tiles"));

            if (hit)
            {
                TileProps newTile = hit.collider.gameObject.GetComponent<TileProps>();

                if (newTile.type == 1)
                {
                    newTile.SwitchType(2);

                    newTile.grow = Mathf.RoundToInt(grow.x + Random.value * (grow.y - grow.x));
                    newTile.freq = freq;

                    newTile.Grow();

                    massTemp -= 1;
                }
            }
        }

        CalcLandTiles();

        AssignID();

        PlaceResources();

        SetAgriResource();

        SetPopulation();

        GenerateNations();

        Invoke(nameof(DrawGrid), Time.deltaTime);
    }

    void GenerateNations()
    {
        List<TileProps> availableTiles = FindObjectsOfType<TileProps>().Where(tile => tile.type != 1).ToList();

        for (int i = 0; i < 2; i++)
        {
            GameObject newNation = Instantiate(nationPrefab);
            NationProps nation = newNation.GetComponent<NationProps>();
            nation.nationName = i == 0 ? "Blue Nation" : "Red Nation";

            int randomIndex = Random.Range(0, availableTiles.Count);
            TileProps randomTile = availableTiles[randomIndex];
            
            randomTile.nation = newNation;
            nation.tiles.Add(randomTile);

            availableTiles.RemoveAt(randomIndex);
        }
    }

    void CalcLandTiles()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector2 pos = new Vector2(x, y * 0.86f);

                if (y % 2 == 0)
                {
                    pos.x += 0.5f;
                }

                RaycastHit2D hit = Physics2D.Raycast(pos, pos, 0, LayerMask.GetMask("Tiles"));
                if (hit)
                {
                    if (hit.collider.gameObject.GetComponent<TileProps>().type > 1)
                    {
                        landTiles += 1;
                        landTilesList.Add(hit.collider.gameObject.GetComponent<TileProps>());
                    }
                }
            }

        }
    }

    void AssignID()
    {
        int i = 0;

        foreach (TileProps tile in landTilesList)
        {
            tile.ID = i;
            i++;
        }
    }

    void SetPopulation()
    {
        foreach (TileProps tile in landTilesList)
        {
            tile.totalPop = Mathf.RoundToInt(Random.Range(10000,100000));
            tile.SetPopulationRatios(30,40,30);
        }
    }

    void SetAgriResource()
    {
        foreach (TileProps tile in landTilesList)
        {
            tile.agriResource = "Grain";
        }
    }

    void PlaceResources() //change this to the new method
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector2 pos = new Vector2(x, y * 0.86f);

                if (y % 2 == 0)
                {
                    pos.x += 0.5f;
                }

                RaycastHit2D hit = Physics2D.Raycast(pos, pos, 0, LayerMask.GetMask("Tiles"));
                if (hit)
                {
                    TileProps tile = hit.collider.gameObject.GetComponent<TileProps>();
                    if (tile.type > 1)
                    {
                        int randomNumber = Random.Range(1, 101);
                        if (randomNumber > 95)
                        {
                            tile.resource = "Gold";
                        }
                        else if (randomNumber > 75)
                        {
                            tile.resource = "Coal";
                        }
                        else if (randomNumber > 50)
                        {
                            tile.resource = "Iron";
                        }
                        else
                        {
                            tile.resource = "Timber";
                        }
                    }
                }
            }
        }
    }

    void DrawGrid()
    {
        GetComponent<PathGrid>().width = (int)mapSize.x;
        GetComponent<PathGrid>().height = (int)mapSize.y;

        GetComponent<PathGrid>().DrawGrid();
    }
}