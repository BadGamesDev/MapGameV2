using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileUI : MonoBehaviour
{
    public GameState gameState;
    public ModifierManager modifierManager; // I... think... this is good?
    public MapGenerator mapGenerator; //not a big fan of referencing this here

    public GameObject panelUI;
    public Button buttonSettleTile; //not a big fan of referencing it like this either
    public Image tileTerrainImage;

    public TMP_Text tileOwnerText;
    public TMP_Text totalPopText;
    public TMP_Text tribalPopText;
    public TMP_Text agriPopText;
    public TMP_Text resourcePopText;
    public TMP_Text industryPopText;
    public TMP_Text migrationText;
    public TMP_Text resourceTypeText; //Thinking of making this an image;

    public void OpenTileUI()
    {
        panelUI.SetActive(true);

        if (gameState.playerNation.GetNationEmptyNeighbors().Contains(gameState.activeTile) || gameState.playerNation.capital == null && gameState.playerNation.navies[0].GetNeighbors().Contains(gameState.activeTile))
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
        
        totalPopText.text = gameState.activeTile.GetTotalPopulation().ToString();
        tribalPopText.text = gameState.activeTile.GetTribalPopulation().ToString();
        agriPopText.text = gameState.activeTile.GetAgriPopulation().ToString();
        resourcePopText.text = gameState.activeTile.GetResourcePopulation().ToString();
        industryPopText.text = gameState.activeTile.GetIndustryPopulation().ToString();

        migrationText.text = Mathf.RoundToInt(gameState.activeTile.migration).ToString();

        resourceTypeText.text = gameState.activeTile.resource;

        //THE PART AFTER THIS NEEDS TO BE FIXED, REALLY UGLY CODE

        if (gameState.playerNation.GetNationEmptyNeighbors().Contains(gameState.activeTile) || gameState.playerNation.capital == null && gameState.playerNation.navies[0].GetNeighbors().Contains(gameState.activeTile))
        {
            buttonSettleTile.interactable = true;
        }

        else
        {
            buttonSettleTile.interactable = false;
        }
    }

    public void SettleTile() //I think this should take in a nation and tile to make it useful for AI etc.
    {
        TileProps settledTile = gameState.activeTile;
        NationProps playerNation = gameState.playerNation;

        if (gameState.playerNation.capital == null)
        {
            playerNation.navies[0].DeleteNavy();
            float tribes = settledTile.tribalPop;
            settledTile.infrastructureLevel = 25;
            settledTile.totalPop = 10000;
            settledTile.SetPopulationRatios(0, 80, 20, 0); //this is also important because pop will revert to the empty tile value if you don't do it like this
            settledTile.tribalPop += tribes;
            settledTile.nation = playerNation;
            playerNation.ownedTiles.Add(settledTile);
            playerNation.capital = settledTile;

            playerNation.discoveredTiles.Add(settledTile);
            playerNation.discoveredTiles.AddRange(playerNation.GetNationNeighbors());

            mapGenerator.ClearFOW();
            UpdateTileUI();

            MainUI mainUI = FindObjectOfType<MainUI>();
            mainUI.settlementMessage.SetActive(true);
            Calendar calendar = FindObjectOfType<Calendar>();
            calendar.chooseSpeed0();
        }

        else if (playerNation.capital.agriPop >= 80 && playerNation.capital.resourcePop >= 20)
        {
            TileProps capitalTile = playerNation.capital;

            settledTile.nation = playerNation; //hard coding feels bad... again

            settledTile.agriPop += 80;
            settledTile.resourcePop += 20;

            capitalTile.agriPop -= 80;
            capitalTile.resourcePop -= 20;

            playerNation.ownedTiles.Add(gameState.activeTile);

            List<TileProps> neighbors = playerNation.GetNationNeighbors(); //No need to get all the nation neighbors, just get tile neighbors

            foreach (TileProps neighbor in neighbors)
            {
                if (!playerNation.discoveredTiles.Contains(neighbor))
                {
                    playerNation.discoveredTiles.Add(neighbor);
                }
            }
            
            Modifier newColonyModifier = new Modifier();
            newColonyModifier.type = ModifierType.AttractionModifier;
            newColonyModifier.intensity = 0.05f;
            newColonyModifier.duration = 1000;

            modifierManager.ApplyModifier(settledTile, newColonyModifier);
            mapGenerator.ClearFOW();
            UpdateTileUI(); //this is temporary!
        }
    }
}
