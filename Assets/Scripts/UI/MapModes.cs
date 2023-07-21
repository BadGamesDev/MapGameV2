using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModes : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public GameState gameState;

    public List<TileProps> tiles;

    public enum Mode
    {
        terrain,
        political,
        resource,
        population,
        recruitment
    }

    public Mode mapMode = Mode.terrain;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T)) //do NOT hardcode keys
        {
            mapMode = Mode.terrain;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            mapMode = Mode.political;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            mapMode = Mode.resource;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            mapMode = Mode.population;
        }

        if (gameState.gameMode == GameState.Mode.recruitModeArmy)
        {
            mapMode = Mode.recruitment;
        }

        if (gameState.gameMode == GameState.Mode.recruitModeTiles)
        {
            mapMode = Mode.recruitment;
        }

        switch (mapMode)
        {
            case Mode.terrain:
                ApplyTerrainMapMode();
                break;
            case Mode.political:
                ApplyPoliticalMapMode();
                break;
            case Mode.population:
                ApplyPopulationMapMode();
                break;
            case Mode.resource:
                ApplyResourceMapMode();
                break;
            case Mode.recruitment:
                ApplyRecruitmentMapMode();
                break;
        }
    }

    private void ApplyTerrainMapMode()
    {
        foreach (TileProps tile in mapGenerator.landTiles)
        {
            tile.SwitchSprite(tile.type);
            tile.GetComponent<Renderer>().material.color = Color.white;

            if (tile.nation != null)
            {
                if (tile.nation.GetComponent<NationProps>().nationName == "0")
                {
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (tile.nation.GetComponent<NationProps>().nationName == "1")
                {
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
        }
    }

    private void ApplyPoliticalMapMode()
    {
        foreach (TileProps tile in mapGenerator.landTiles)
        {
            if (tile.nation != null)
            {
                if (tile.nation.GetComponent<NationProps>().nationName == "0")
                {
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (tile.nation.GetComponent<NationProps>().nationName == "1")
                {
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
            else
            {
                tile.SwitchSprite(0);
                tile.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    private void ApplyResourceMapMode() //might add resource sprites later
    {
        foreach (TileProps tile in mapGenerator.landTiles)
        {
            if (tile.resource == "Timber")
            {
                tile.SwitchSprite(0);
                tile.GetComponent<Renderer>().material.color = Color.green;
            }
            else if (tile.resource == "Iron")
            {
                tile.SwitchSprite(0);
                tile.GetComponent<Renderer>().material.color = Color.gray;
            }
            else if (tile.resource == "Coal")
            {
                tile.SwitchSprite(0);
                tile.GetComponent<Renderer>().material.color = Color.black;
            }
            else if (tile.resource == "Gold")
            {
                tile.SwitchSprite(0);
                tile.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                tile.SwitchSprite(0);
                tile.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    private void ApplyRecruitmentMapMode()
    {
        foreach (TileProps tile in mapGenerator.landTiles)
        {
            if (tile.nation == gameState.playerNation && tile.isReinforceTile == false)
            {
                tile.SwitchSprite(0);
                tile.GetComponent<Renderer>().material.color = Color.green;
            }

            else if (tile.nation == gameState.playerNation && tile.isReinforceTile == true)
            {
                if (gameState.activeArmy == null) //doesn't feel right, maybe it can be simplified
                {
                        tile.SwitchSprite(0);
                        tile.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    if (tile.nation.armies.Find(army => army == gameState.activeArmy).reinforceTiles.Contains(tile))
                    {
                        tile.SwitchSprite(0);
                        tile.GetComponent<Renderer>().material.color = Color.magenta;
                    }
                    else
                    {
                        tile.SwitchSprite(0);
                        tile.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
            else
            {
                tile.SwitchSprite(0);
                tile.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    private void ApplyPopulationMapMode()
    {
        float maxPopulation = 0f;
        foreach (TileProps tile in mapGenerator.landTiles)
        {
            float population = tile.GetTotalPopulation();
            if (population > maxPopulation)
            {
                maxPopulation = population;
            }
        }

        foreach (TileProps tile in mapGenerator.landTiles)
        {
            tile.SwitchSprite(0);
            float population = tile.GetTotalPopulation();

            float populationRatio = population / maxPopulation;

            Color color = Color.Lerp(Color.white, Color.green, populationRatio);

            // Set the sprite color of the tile
            tile.GetComponent<Renderer>().material.color = color;
        }
    }
}