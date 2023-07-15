using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileUI : MonoBehaviour
{
    public GameState gameState;
    public MapGenerator mapGenerator; //not a big fan of referencing this here

    public GameObject panelUI;
    public Button buttonSettleTile; //not a big fan of referencing it like this either
    public Image tileTerrainImage;

    public TMP_Text tileOwnerText;
    public TMP_Text totalPopText;
    public TMP_Text agriPopText;
    public TMP_Text resourcePopText;
    public TMP_Text industryPopText;
    public TMP_Text migrationText;
    public TMP_Text resourceTypeText; //Thinking of making this an image;

    public void OpenTileUI()
    {
        panelUI.SetActive(true);

        if (gameState.playerNation.GetNationEmptyNeighbors().Contains(gameState.activeTile))
        {
            buttonSettleTile.interactable = true;
        }

        else
        { 
            buttonSettleTile.interactable = false; 
        }
    }

    public void CloseTileUI()
    {
        panelUI.SetActive(false);
    }

    public void UpdateTileUI()
    {
        if (gameState.activeTile.nation != null)
        {
            tileOwnerText.text = gameState.activeTile.nation.name;
        }
        else
        {
            tileOwnerText.text = "Unoccupied";
        }

        totalPopText.text = gameState.activeTile.totalPop.ToString();
        agriPopText.text = gameState.activeTile.agriPop.ToString();
        resourcePopText.text = gameState.activeTile.resourcePop.ToString();
        industryPopText.text = gameState.activeTile.industryPop.ToString();

        migrationText.text = gameState.activeTile.migration.ToString();

        resourceTypeText.text = gameState.activeTile.resource;
    }

    public void SettleTile()
    {
        TileProps settledTile = gameState.activeTile;
        TileProps capitalTile = gameState.playerNation.capital;
        
        settledTile.nation = gameState.playerNation; //hard coding feels bad... again
        settledTile.IncreasePopulationFlat(100);
        capitalTile.DecreasePopulationFlat(100);


        gameState.playerNation.ownedTiles.Add(gameState.activeTile);

        List<TileProps> neighbors = gameState.playerNation.GetNationNeighbors(); //No need to get all the nation neighbors, just get tile neighbors

        foreach (TileProps neighbor in neighbors)
        {
            if (!gameState.playerNation.discoveredTiles.Contains(neighbor))
            {
                gameState.playerNation.discoveredTiles.Add(neighbor);
            }
        }

        mapGenerator.ClearFOW();
    }
}
