using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileUI : MonoBehaviour
{
    public GameState gameState;

    public GameObject panelUI;
    public Image tileTerrainImage;

    public TMP_Text totalPopText;
    public TMP_Text agriPopText;
    public TMP_Text resourcePopText;
    public TMP_Text industryPopText;

    public TMP_Text resourceTypeText; //Thinking of making this an image;

    public void OpenTileUI()
    {
        panelUI.SetActive(true);
    }

    public void CloseTileUI()
    {
        panelUI.SetActive(false);
    }

    public void SettleTile()
    {
        gameState.activeTile.nation = gameState.playerNation; //hard coding feels bad... again
        gameState.playerNation.tiles.Add(gameState.activeTile);
    }

    public void UpdateTileUI()
    {
        totalPopText.text = gameState.activeTile.totalPop.ToString();
        agriPopText.text = gameState.activeTile.agriPop.ToString();
        resourcePopText.text = gameState.activeTile.resourcePop.ToString();
        industryPopText.text = gameState.activeTile.industryPop.ToString();

        resourceTypeText.text = gameState.activeTile.resource;
    }
}
