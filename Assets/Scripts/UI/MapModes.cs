using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModes : MonoBehaviour
{
    public enum Mode
    {
        terrain,
        political,
        resource
    }

    public Mode currentMode = Mode.terrain;
    public List<TileProps> tiles;

    private void Start()
    {
        tiles = new List<TileProps>(FindObjectsOfType<TileProps>());
    }

    private void Update()
    {
        //These will probably by moved to another script
        if (Input.GetKeyDown(KeyCode.T))
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
        //#################################################
        switch (currentMode)
        {
            case Mode.terrain:
                ApplyTerrainMapMode();
                break;

            case Mode.political:
                ApplyPoliticalMapMode();
                break;
            case Mode.resource:
                ApplyResourceMapMode();
                break;
        }
    }

    private void ApplyTerrainMapMode()
    {
        foreach (TileProps tile in tiles)
        {
            tile.SwitchSprite(tile.type);

            tile.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    private void ApplyPoliticalMapMode()
    {
        foreach (TileProps tile in tiles)
        {
            if (tile.nation == null)
            {

            }
            else if (tile.nation.GetComponent<NationProps>().nationName == "Blue Nation")
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
    }

    private void ApplyResourceMapMode()
    {
        foreach (TileProps tile in tiles)
        {
            if (tile.type != 1) //this is a temporary solution make a separate land tiles list
            {
                if (tile.resource == "Timber")
                {
                    //tile.SwitchSprite(0); RESOURCE SPRITES WILL BE ADDED
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (tile.resource == "Iron")
                {
                    //tile.SwitchSprite(0); RESOURCE SPRITES WILL BE ADDED
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.gray;
                }
                else if (tile.resource == "Coal")
                {
                    //tile.SwitchSprite(0); RESOURCE SPRITES WILL BE ADDED
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.black;
                }
                else if (tile.resource == "Gold")
                {
                    //tile.SwitchSprite(0); RESOURCE SPRITES WILL BE ADDED
                    tile.SwitchSprite(0);
                    tile.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else
                {
                        tile.SwitchSprite(0);
                }
            }
        }
    }
}