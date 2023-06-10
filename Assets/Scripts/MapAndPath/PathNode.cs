using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector2 pos;

    public float gCost; //terrain etc. cost
    public float hCost; //distance cost
    public float fCost; //total cost

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
