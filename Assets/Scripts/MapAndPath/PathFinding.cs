using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public PathGrid grid;

    List<PathNode> openNodes = new List<PathNode>();
    List<PathNode> closedNodes = new List<PathNode>();

    private void Start()
    {
        grid = GetComponent<PathGrid>();
    }
    
    public List<Vector2> GetPath(Vector2 startPos, Vector2 endPos, int unitType)
    {
        List<Vector2> pathVect = new List<Vector2>();

        PathNode startNode = grid.GetNode(startPos);
        PathNode endNode = grid.GetNode(endPos);

        if (startNode == null || endNode == null)
        {
            Debug.Log("Invalid Position");
            pathVect.Add(startPos);
            return pathVect;
        }

        openNodes = new List<PathNode> { startNode };
        closedNodes = new List<PathNode>();

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                PathNode pathNode = grid.GetNodeInt(x, y);
                pathNode.gCost = 999999;
                pathNode.CalcFcost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalcDistance(startNode.pos, endNode.pos);
        startNode.CalcFcost();

        while (openNodes.Count > 0)
        {
            PathNode currNode = GetLowestF();
            if (currNode == endNode)
            {
                return CalcPath(currNode);
            }

            openNodes.Remove(currNode);
            closedNodes.Add(currNode);

            foreach (PathNode neighbor in GetNeighbors(currNode))
            {
                if (closedNodes.Contains(neighbor))
                {
                    continue;
                }
                if (neighbor.open > unitType)
                {
                    closedNodes.Add(neighbor);
                    continue;
                }
                else
                {
                    float newGcost = currNode.gCost + 1;

                    if (newGcost < neighbor.gCost)
                    {
                        neighbor.cameFromNode = currNode;
                        neighbor.gCost = newGcost;
                        neighbor.hCost = CalcDistance(neighbor.pos, endNode.pos);
                        neighbor.CalcFcost();

                        if (!openNodes.Contains(neighbor))
                        {
                            openNodes.Add(neighbor);
                        }
                    }
                }
            }
        }

        PathNode closestNode = GetClosestNode(endNode);
        if (closestNode != null)
        {
            return CalcPath(closestNode);
        }

        Debug.Log("No Path Found");
        pathVect.Add(startPos);
        return pathVect;
    }

    private float CalcDistance(Vector2 a, Vector2 b)
    {
        float xDist = Mathf.Abs(a.x - b.x);
        float yDist = Mathf.Abs(a.y - b.y);

        return xDist + yDist;
    }
    
    private PathNode GetLowestF()
    {
        PathNode lowestF = openNodes[0];
        for (int n = 0; n < openNodes.Count; n++)
        {
            if (openNodes[n].fCost < lowestF.fCost)
            {
                lowestF = openNodes[n];
            }
        }
        return lowestF;
    }

    private List<PathNode> GetNeighbors(PathNode currNode)
    {
        List<PathNode> neighborList = new List<PathNode>();

        if (currNode.pos.x - 1 >= 0) //can move left
        {
            neighborList.Add(grid.GetNode(currNode.pos + new Vector2(-1, 0)));
        }
        
        if (currNode.pos.x + 1 < grid.width) //can move right
        {
            neighborList.Add(grid.GetNode(currNode.pos + new Vector2(1, 0)));
        }

        if (Mathf.RoundToInt((currNode.pos.y + 0.86f) / 0.86f) < grid.height)
        {
            if(currNode.pos.x - 0.5f >= 0) //can move upper left
            {
                neighborList.Add(grid.GetNode(currNode.pos + new Vector2(-0.5f, 0.86f)));
            }

            if (currNode.pos.x + 0.5f < grid.width) //can move upper right
            {
                neighborList.Add(grid.GetNode(currNode.pos + new Vector2(0.5f, 0.86f)));
            }
        }

        if (currNode.pos.y - 0.86f >= 0)
        {
            if (currNode.pos.x - 0.5f >= 0) //can move bottom left
            {
                neighborList.Add(grid.GetNode(currNode.pos + new Vector2(-0.5f, -0.86f)));
            }

            if (currNode.pos.x + 0.5f < grid.width) //can move bottom right
            {
                neighborList.Add(grid.GetNode(currNode.pos + new Vector2(0.5f, -0.86f)));
            }
        }

        return neighborList;
    }

    private List<Vector2> CalcPath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currNode = endNode;
        while (currNode.cameFromNode != null)
        {
            path.Add(currNode.cameFromNode);
            currNode = currNode.cameFromNode;
        }
        path.Reverse();

        List<Vector2> pathVector = new List<Vector2>();

        for (int n = 0; n < path.Count; n++)
        {
            pathVector.Add(path[n].pos);
        }
        return pathVector;
    }

    private PathNode GetClosestNode(PathNode targetNode)
    {
        PathNode closestNode = null;
        float closestDistance = float.MaxValue;

        foreach (PathNode node in closedNodes)
        {
            float distance = CalcDistance(node.pos, targetNode.pos);
            if (distance < closestDistance && node.open <= 9)
            {
                closestNode = node;
                closestDistance = distance;
            }
        }

        return closestNode;
    }
}

