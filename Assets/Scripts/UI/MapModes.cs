using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModes : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public GameState gameState;

    public enum Mode
    {
        terrain,
        political,
        resource,
        population,
        recruitment
    }

    public Mode currentMode = Mode.terrain;
    public List<TileProps> tiles;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T)) //do NOT hardcode keys
        {
            currentMode = Mode.terrain;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            currentMode = Mode.political;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentMode = Mode.resource;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            currentMode = Mode.population;
        }

        if (gameState.recruitModeArmy)
        {
            currentMode = Mode.recruitment;
        }

        switch (currentMode)
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
        foreach (TileProps tile in mapGenerator.landTilesList)
        {
            tile.SwitchSprite(tile.type);
            tile.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    private void ApplyPoliticalMapMode()
    {
        foreach (TileProps tile in mapGenerator.landTilesList)
        {
            if (tile.nation != null)
            {
                if (tile.nation.GetComponent<NationProps>().nationName == "Blue Nation")
                {
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else if (tile.nation.GetComponent<NationProps>().nationName == "Red Nation")
                {
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.red;
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
        foreach (TileProps tile in mapGenerator.landTilesList)
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
        foreach (TileProps tile in mapGenerator.landTilesList)
        {
                if (tile.nation != null && tile.isReinforceTile == false)
                {
                        tile.SwitchSprite(0);
                        tile.GetComponent<Renderer>().material.color = Color.green;
                }
                
                else if (tile.nation != null && tile.isReinforceTile == true)
                {
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.red;
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
        float minPopulation = Mathf.Infinity;
        float maxPopulation = 0f;
        foreach (TileProps tile in mapGenerator.landTilesList)
        {
            float population = tile.GetTotalPopulation();
            if (population < minPopulation)
            {
                minPopulation = population;
            }
            if (population > maxPopulation)
            {
                maxPopulation = population;
            }
        }

        foreach (TileProps tile in mapGenerator.landTilesList)
        {
            tile.SwitchSprite(0);
            float population = tile.GetTotalPopulation();

            float populationRatio = (population - minPopulation) / (maxPopulation - minPopulation);

            Color color = Color.Lerp(Color.green, Color.white, populationRatio);

            // Set the sprite color of the tile
            tile.GetComponent<Renderer>().material.color = color;
        }
    }
}