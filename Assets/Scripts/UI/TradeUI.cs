using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradeUI : MonoBehaviour
{
    public GameState gameState; //this can be done in update manager for an extremely small improvement in performance
    public MainUI mainUI;

    public ResourceManager resourceManager;

    public List<TMP_Text> supplyTexts; //lists are filled manually in the editor
    public List<TMP_Text> demandTexts;
    public List<TMP_Text> globalSupplyTexts;
    public List<TMP_Text> globalDemandTexts;

    public void UpdateSupplyDemandDisplay() // add an if statement so that it is only updated when open
    {
        ClearTexts(supplyTexts);
        ClearTexts(demandTexts);
        ClearTexts(globalSupplyTexts);
        ClearTexts(globalDemandTexts);

        foreach (var supplyItem in gameState.playerNation.supply)
        {
            string resourceName = supplyItem.Key;
            float supplyAmount = supplyItem.Value;
            TMP_Text supplyText = GetTextObject(supplyTexts, resourceName);
            supplyText.text = $"{resourceName}: {mainUI.FormatNumberResource(supplyAmount)}"; // might be bad for performance
        }

        foreach (var demandItem in gameState.playerNation.demand)
        {
            string resourceName = demandItem.Key;
            float demandAmount = demandItem.Value;
            TMP_Text demandText = GetTextObject(demandTexts, resourceName); // I feel like I should just do this once
            demandText.text = $"{resourceName}: {mainUI.FormatNumberResource(demandAmount)}";
        }

        foreach (var supplyItem in resourceManager.globalSupply) // THERE IS A BUG HERE FOR SOME FUCKING REASON
        {
            string resourceName = supplyItem.Key;
            float supplyAmount = supplyItem.Value;
            TMP_Text supplyText = GetTextObject(globalSupplyTexts, resourceName);
            supplyText.text = $"{resourceName}: {mainUI.FormatNumberResource(supplyAmount)}";
        }

        foreach (var demandItem in resourceManager.globalDemand)
        {
            string resourceName = demandItem.Key;
            float demandAmount = demandItem.Value;
            TMP_Text demandText = GetTextObject(globalDemandTexts, resourceName);
            demandText.text = $"{resourceName}: {mainUI.FormatNumberResource(demandAmount)}";
        }
    }

    public TMP_Text GetTextObject(List<TMP_Text> texts, string resourceName)
    {
        TMP_Text textObject = texts.Find(text => text.name.Equals(resourceName));

        if (textObject == null)
        {
            Debug.LogWarning($"No TMP_Text object found for resource: {resourceName}");
        }

        return textObject;
    }

    public void ClearTexts(List<TMP_Text> texts)
    {
        foreach (var text in texts)
        {
            text.text = string.Empty;
        }
    }
}
