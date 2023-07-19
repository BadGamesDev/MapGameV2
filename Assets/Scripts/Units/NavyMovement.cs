using System.Collections.Generic;
using UnityEngine;

public class NavyMovement : MonoBehaviour
{
    public NavyTracker navyTracker;
    public NavyProps navyProps;
    public TimeManager timeManager;
    public MapGenerator mapGenerator; //I REALLY don't like referencing this shit everywhere

    public List<Vector2> path = new List<Vector2>();

    public int currentNode = 0;
    public float progress = 0;

    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        navyTracker = FindObjectOfType<NavyTracker>();
        //navyTracker.AddNavy(navyProps, transform.position);
    }

    public void MarchNavy() //called in updateManager everyday
    {
        if (path.Count > 0 && navyProps.isInBattle == false)
        {
            if (progress >= 5)
            {
                int tileLayer = LayerMask.GetMask("Tiles");
                RaycastHit2D hit = Physics2D.Raycast(path[currentNode + 1], path[currentNode + 1], 0, tileLayer); //Index out of range bug, happens when on the left or right side of water and you click on water
                if (hit)
                {
                    MoveTo(hit.collider.gameObject);
                    List<TileProps> neighbors = navyProps.GetNeighbors();

                    bool hasFOW = neighbors.Exists(tile => tile.FOW == true);

                    if (hasFOW)
                    {
                        foreach (TileProps neighbor in neighbors)
                        {
                            if (!navyProps.nation.discoveredTiles.Contains(neighbor))
                            {
                                navyProps.nation.discoveredTiles.Add(neighbor);
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
                    currentNode = 0;
                    progress = 5;
                }
            }
            else
            {
                progress += navyProps.speed;
            }
        }
    }

    void MoveTo(GameObject tile)
    {
        transform.position = tile.transform.position;

        progress = 0;
        currentNode += 1;

        if (currentNode >= path.Count - 1)
        {
            path = new List<Vector2>();
            currentNode = 0;
            progress = 0.0f;
        }

        navyTracker.UpdateNavyPosition(navyProps, transform.position);
    }
}
