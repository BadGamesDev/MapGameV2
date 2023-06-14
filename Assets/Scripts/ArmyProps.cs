using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyProps : MonoBehaviour
{
    public NationProps nation;
    public List<TileProps> reinforceTiles = new List<TileProps>();

    public int desiredSize;
    public int curSize;
    public int availablePop;
}
