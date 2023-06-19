using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    public TimeManager timeManager;
    public List<Vector2> path = new List<Vector2>();
    public int currNode = 0;
    public float delay = 5;

    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }
    private void Update()
    {
        if (path.Count > 0)
        {
            if(delay >= 5)
            {
                RaycastHit2D hit = Physics2D.Raycast(path[currNode], path[currNode], 0, LayerMask.GetMask("Tiles"));
                if (hit)
                {
                    MoveTo(hit.collider.gameObject);
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
                delay += Time.deltaTime * timeManager.timeMultiplier;
            }
        }
    }

    void MoveTo(GameObject tile)
    {
        transform.position = tile.transform.position;
        transform.parent = tile.transform;

        delay = 0;

        currNode += 1;

        if(currNode >= path.Count)
        {
            path = new List<Vector2>();
            currNode = 0;
            delay = 0.5f;
        }
    }
}

