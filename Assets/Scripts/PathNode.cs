using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector2 pos;

    public float gCost;
    public float hCost;
    public float fCost;

    public PathNode cameFromNode;

    public int open = 0;
    public int oldOpen = 0;

    public PathNode(Vector2 pos)
    {
        this.pos = pos;
    }

    public void CalcFcost()
    {
        fCost = gCost + hCost;
    }
}
