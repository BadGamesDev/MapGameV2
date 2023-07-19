using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    public Dictionary<TileProps, Modifier> tileModifiers = new Dictionary<TileProps, Modifier>(); // I might turn this into an event and keep the dictionary in the update manager

    public void ApplyModifier(TileProps tile, Modifier modifier)
    {
        switch (modifier.type)
        {
            case ModifierType.AttractionModifier:
                ApplyAttractionModifier(tile, modifier);
                break;
            case ModifierType.GrowthModifier:
                ApplyGrowthModifier(tile, modifier);
                break;
        }
        
        tileModifiers.Add(tile, modifier);
    }

    public void RemoveModifier(TileProps tile, Modifier modifier)
    {
        switch (modifier.type)
        {
            case ModifierType.AttractionModifier:
                RemoveAttractionModifier(tile, modifier);
                break;
            case ModifierType.GrowthModifier:
                RemoveGrowthModifier(tile, modifier);
                break;
        }

        tileModifiers.Remove(tile);
    }

    private void ApplyAttractionModifier(TileProps tile, Modifier modifier) //I mean... It works fine. There is no problem. The code might get a bit long but it is clean.
    {
        tile.attractionModifier += modifier.intensity;
    }

    private void RemoveAttractionModifier(TileProps tile, Modifier modifier)
    {
        tile.attractionModifier -= modifier.intensity;
    }

    private void ApplyGrowthModifier(TileProps tile, Modifier modifier)
    {
        tile.popGrowthRateNonTribal += modifier.intensity;
    }

    private void RemoveGrowthModifier(TileProps tile, Modifier modifier)
    {
        tile.popGrowthRateNonTribal -= modifier.intensity;
    }
}

public class Modifier
{
    public ModifierType type;
    public int duration;
    public float intensity;
}

public enum ModifierType
{
    None,
    AttractionModifier,
    GrowthModifier,
}
