using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyMovement : MonoBehaviour
{
    private ArmyTracker armyTracker;
    public ArmyProps armyProps;
    public TimeManager timeManager;
    public MapGenerator mapGenerator; //I REALLY don't like referencing this shit everywhere

    public List<Vector2> path = new List<Vector2>();

    public int currNode = 0;
    public float delay = 0;

    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        armyTracker = ArmyTracker.instance;
        armyTracker.AddArmy(armyProps, transform.position);
    }

    public void MarchArmy() //called in updateManager everyday
    {
        if (path.Count > 0)
        {
            if (delay >= 5)
            {
                int tileLayer = LayerMask.GetMask("Tiles");
                RaycastHit2D hit = Physics2D.Raycast(path[currNode+1], path[currNode+1], 0, tileLayer);
                if (hit)
                {
                    MoveTo(hit.collider.gameObject);
                    List<TileProps> neighbors = armyProps.GetNeighbors();

                    bool hasFOW = neighbors.Exists(tile => tile.FOW == true);

                    if (hasFOW)
                    {
                        foreach (TileProps neighbor in neighbors)
                        {
                            if (!armyProps.nation.discoveredTiles.Contains(neighbor))
                            {
                                armyProps.nation.discoveredTiles.Add(neighbor);
                            }
                        }
                        mapGenerator.ClearFOW();
                    }
                    else
                    {
                        Debug.Log("None of the neighbors have fog of war enabled.");
                    }
                }
                else
                {
                    Debug.Log("No Tile Found");
                    path = new List<Vector2>();
                    currNode = 0;
                    delay = 5;
                }
            }
            else
            {
                delay += 1;
            }
        }
    }

    void MoveTo(GameObject tile)
    {
        transform.position = tile.transform.position;
        transform.parent = tile.transform;

        delay = 0;
        currNode += 1;

        armyTracker.UpdateArmyPosition(armyProps, transform.position);

        if (currNode >= path.Count - 1)
        {
            path = new List<Vector2>();
            currNode = 0;
            delay = 0.0f;
        }
    }
}