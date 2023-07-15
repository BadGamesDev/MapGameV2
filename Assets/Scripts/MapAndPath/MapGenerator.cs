using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameState gameState;

    public GameObject fowPrefab;
    public GameObject tilePrefab;
    public GameObject nationPrefab;
    public GameObject tilesParent;

    public int masses = 30;

    public Vector2 mapSize = new Vector2(80, 40);

    public Vector2 grow = new Vector2(4, 7);
    public int freq = 3;

    public int landTileCount = 0;

    public List<TileProps> landTiles;

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

                tilePrefab.GetComponent<TileProps>().SwitchType(1);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, tilesParent.transform);

                GameObject fog = Instantiate(fowPrefab, tile.transform.position, Quaternion.identity, tile.transform);
                TileProps tileProps = tile.GetComponent<TileProps>();
                tileProps.FOW = fog;
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

        SetAgriResources();

        SetLaborResources();

        SetPopulation();

        SetDevelopment();

        GenerateNations();

        ClearFOW();

        Invoke(nameof(DrawGrid), Time.deltaTime);
    }

    public void ClearFOW() //referenced in army movement
    {
        foreach (TileProps tile in gameState.playerNation.discoveredTiles) //this can be obtimised but the impact on performance is fine for now
        {
            if (tile.FOW.activeSelf)
            {
                tile.FOW.SetActive(false);
            }
        }
    }

    void GenerateNations()
    {
        List<TileProps> availableTiles = FindObjectsOfType<TileProps>().Where(tile => tile.type != 1).ToList();

        for (int i = 0; i < 2; i++)
        {
            GameObject newNation = Instantiate(nationPrefab);
            NationProps nation = newNation.GetComponent<NationProps>();
            
            newNation.name = "Nation" + i.ToString();
            nation.nationName = i.ToString();
            nation.isAI = true;

            int randomIndex = Random.Range(0, availableTiles.Count);
            TileProps randomTile = availableTiles[randomIndex];

            randomTile.infrastructureLevel = 25;
            randomTile.totalPop = 20000;
            randomTile.SetPopulationRatios(0,80,20,0); //this is also important because pop will revert to the empty tile value if you don't do it like this
            randomTile.nation = nation;
            nation.ownedTiles.Add(randomTile);
            nation.capital = randomTile;

            availableTiles.RemoveAt(randomIndex);

            nation.discoveredTiles.Add(randomTile);
            nation.discoveredTiles.AddRange(nation.GetNationNeighbors());
        }

        gameState.ChoosePlayerNation();
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
                        landTileCount += 1;
                        landTiles.Add(hit.collider.gameObject.GetComponent<TileProps>());
                    }
                }
            }

        }
    }

    void AssignID()//not used for now
    {
        int i = 0;

        foreach (TileProps tile in landTiles)
        {
            tile.ID = i;
            i++;
        }
    }

    void SetPopulation()
    {
        foreach (TileProps tile in landTiles)
        {
            tile.totalPop = Mathf.RoundToInt(Random.Range(100, 1001));
            tile.SetPopulationRatios(100, 0, 0, 0);
        }
    }

    void SetDevelopment() // might make this weighted by pops in the future
    {
        foreach (TileProps tile in landTiles)
        {
            tile.infrastructureLevel = Random.Range(5, 11);
        }
    }

    void SetAgriResources() //MORE WILL BE ADDED
    {
        foreach (TileProps tile in landTiles)
        {
            tile.agriResource = "Grain";
        }
    }

    void SetLaborResources()
    {
        foreach(TileProps tile in landTiles) 
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

    void DrawGrid()
    {
        GetComponent<PathGrid>().width = (int)mapSize.x;
        GetComponent<PathGrid>().height = (int)mapSize.y;

        GetComponent<PathGrid>().DrawGrid();
    }
}