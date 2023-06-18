using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class SupplyDemandUI : MonoBehaviour
{
    public MainUI mainUI;

    public List<TMP_Text> supplyTexts;
    public List<TMP_Text> demandTexts;
    public List<TMP_Text> globalSupplyTexts;
    public List<TMP_Text> globalDemandTexts;

    public NationProps nation;
    public ResourceManager resourceManager;

    private void Update()
    {
        nation = FindObjectOfType<NationProps>();
        UpdateSupplyDemandDisplay();
    }

    private void UpdateSupplyDemandDisplay()
    {
        ClearTexts(supplyTexts);
        ClearTexts(demandTexts);
        ClearTexts(globalSupplyTexts);
        ClearTexts(globalDemandTexts);

        foreach (var supplyItem in nation.supply)
        {
            string resourceName = supplyItem.Key;
            float supplyAmount = supplyItem.Value;
            TMP_Text supplyText = GetTextObject(supplyTexts, resourceName);
            supplyText.text = $"{resourceName}: {mainUI.FormatNumber(Mathf.RoundToInt(supplyAmount))}"; // might be bad for performance
        }

        foreach (var demandItem in nation.demand)
        {
            string resourceName = demandItem.Key;
            float demandAmount = demandItem.Value;
            TMP_Text demandText = GetTextObject(demandTexts, resourceName);
            demandText.text = $"{resourceName}: {mainUI.FormatNumber(Mathf.RoundToInt(demandAmount))}";
        }

        foreach (var supplyItem in resourceManager.globalSupply)
        {
            string resourceName = supplyItem.Key;
            float supplyAmount = supplyItem.Value;
            TMP_Text supplyText = GetTextObject(globalSupplyTexts, resourceName);
            supplyText.text = $"{resourceName}: {mainUI.FormatNumber(Mathf.RoundToInt(supplyAmount))}";
        }

        foreach (var demandItem in resourceManager.globalDemand)
        {
            string resourceName = demandItem.Key;
            float demandAmount = demandItem.Value;
            TMP_Text demandText = GetTextObject(globalDemandTexts, resourceName);
            demandText.text = $"{resourceName}: {mainUI.FormatNumber(Mathf.RoundToInt(demandAmount))}";
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
