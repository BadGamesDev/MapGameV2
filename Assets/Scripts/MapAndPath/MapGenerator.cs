using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MapGenerator : MonoBehaviour
{
    public GameState gameState;

    public GameObject fowPrefab;
    public GameObject tilePrefab;
    public GameObject nationPrefab;
    public GameObject tilesParent;
    public GameObject shipPrefab; //Doesn't feel right here

    public int masses;

    public Vector2 mapSize;

    public Vector2 grow;
    public int freq = 3;

    public int landTileCount = 0;
    public int seaTileCount = 0;

    public List<TileProps> landTiles;
    public List<TileProps> seaTiles;

    public List<TileProps> starterTiles; //will be removed, just for the ship

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

            Vector2 pos = new Vector2(Random.Range(14, mapSize.x - 15), Random.Range(14, mapSize.y - 15));

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

        CalcLandTypes();

        GetOuterTiles(); //will be removed

        AssignID();

        SetAgriResources();

        SetLaborResources();

        SetPopulation();

        SetDevelopment();

        SetAgressiveness();

        GenerateNations();

        PlaceStartingBoat();

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
        List<TileProps> availableTiles = FindObjectsOfType<TileProps>().Where(tile => tile.type != 1).ToList(); //just use landtiles instead

        for (int i = 0; i < 2; i++)
        {
            GameObject newNation = Instantiate(nationPrefab);
            NationProps nation = newNation.GetComponent<NationProps>();

            newNation.name = "Nation" + i.ToString();
            nation.nationName = i.ToString();
            nation.isAI = true;

            //int randomIndex = Random.Range(0, availableTiles.Count);
            //TileProps randomTile = availableTiles[randomIndex];

            //randomTile.infrastructureLevel = 25;
            //randomTile.totalPop = 20000;
            //randomTile.SetPopulationRatios(0,80,20,0); //this is also important because pop will revert to the empty tile value if you don't do it like this
            //randomTile.nation = nation;
            //nation.ownedTiles.Add(randomTile);
            //nation.capital = randomTile;

            //availableTiles.RemoveAt(randomIndex);

            //nation.discoveredTiles.Add(randomTile);
            //nation.discoveredTiles.AddRange(nation.GetNationNeighbors());
        }

        gameState.ChoosePlayerNation();
    }

    void PlaceStartingBoat()
    {
        TileProps startTile = starterTiles[Random.Range(0, starterTiles.Count + 1)];
        GameObject starterNavyObject = Instantiate(shipPrefab, new Vector2(startTile.transform.position.x, startTile.transform.position.y), Quaternion.identity);
        NavyProps starterNavy = starterNavyObject.GetComponent<NavyProps>();
        NationProps playerNation = gameState.playerNation;
        UpdateManager updateManager = FindObjectOfType<UpdateManager>(); // very scuffed reference. Do not reference updatemanager from here

        updateManager.navies.Add(starterNavy);

        starterNavy.nation = playerNation;
        playerNation.navies.Add(starterNavy);

        playerNation.discoveredTiles.Add(startTile);

        List<TileProps> neighbors = starterNavy.GetNeighbors();
        foreach (TileProps neighbor in neighbors)
        {
            if (!starterNavy.nation.discoveredTiles.Contains(neighbor))
            {
                starterNavy.nation.discoveredTiles.Add(neighbor);
            }
        }
        ClearFOW();
    }

    void CalcLandTypes()
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
                    else
                    {
                        seaTileCount += 1;
                        seaTiles.Add(hit.collider.gameObject.GetComponent<TileProps>());
                    }
                }
            }

        }
    }

    void GetOuterTiles() //this is a very scuffed temporary method used to make sure the starting ship is far away from the center of the map
    {
        foreach(TileProps tile in seaTiles)
        {
            if(1.5f < tile.transform.position.x && tile.transform.position.x < 78f && 2f < tile.transform.position.y && tile.transform.position.y < 5f)
            {
                starterTiles.Add(tile);
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
        foreach (TileProps tile in landTiles)
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

    void SetAgressiveness()
    {
        foreach (TileProps tile in landTiles)
        {
            int randomNumber = Random.Range(1, 21);
            tile.nativeAgressiveness = randomNumber;
        }
    }
    
    void DrawGrid()
    {
        GetComponent<PathGrid>().width = (int)mapSize.x;
        GetComponent<PathGrid>().height = (int)mapSize.y;

        GetComponent<PathGrid>().DrawGrid();
    }
}