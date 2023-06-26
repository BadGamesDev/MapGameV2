using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileUI : MonoBehaviour
{
    public GameState gameState;

    public Image tileTerrainImage;

    public TMP_Text totalPopText;
    public TMP_Text agriPopText;
    public TMP_Text resourcePopText;
    public TMP_Text industryPopText;

    public TMP_Text resourceTypeText; //Thinking of making this an image;

    public void UpdateTileUI()
    {
        totalPopText.text = gameState.activeTile.totalPop.ToString();
        agriPopText.text = gameState.activeTile.agriPop.ToString();
        resourcePopText.text = gameState.activeTile.resourcePop.ToString();
        industryPopText.text = gameState.activeTile.industryPop.ToString();

        resourceTypeText.text = gameState.activeTile.resource;
    }
}
