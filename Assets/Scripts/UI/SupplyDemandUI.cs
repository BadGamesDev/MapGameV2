using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class SupplyDemandUI : MonoBehaviour
{
    public List<TMP_Text> supplyTexts; // List of TMP_Text objects for displaying supply
    public List<TMP_Text> demandTexts; // List of TMP_Text objects for displaying demand
    public NationProps nation;
    public ResourceManager resourceManager;

    private void Update()
    {
        nation = FindObjectOfType<NationProps>();
        UpdateSupplyDemandDisplay();
    }

    private void UpdateSupplyDemandDisplay()
    {
        // Clear previous values
        ClearTexts(supplyTexts);
        ClearTexts(demandTexts);

        // Display supply values
        foreach (var supplyItem in nation.supply)
        {
            string resourceName = supplyItem.Key;
            float supplyAmount = supplyItem.Value;
            TMP_Text supplyText = GetTextObject(supplyTexts, resourceName);
            supplyText.text = $"{resourceName}: {supplyAmount}";
        }

        // Display demand values
        foreach (var demandItem in nation.demand)
        {
            string resourceName = demandItem.Key;
            float demandAmount = demandItem.Value;
            TMP_Text demandText = GetTextObject(demandTexts, resourceName);
            demandText.text = $"{resourceName}: {demandAmount}";
        }
    }

    private void ClearTexts(List<TMP_Text> texts)
    {
        foreach (var text in texts)
        {
            text.text = string.Empty;
        }
    }

    private TMP_Text GetTextObject(List<TMP_Text> texts, string resourceName)
    {
        TMP_Text textObject = texts.Find(text => text.name.Equals(resourceName));

        if (textObject == null)
        {
            Debug.LogWarning($"No TMP_Text object found for resource: {resourceName}");
        }

        return textObject;
    }
}
