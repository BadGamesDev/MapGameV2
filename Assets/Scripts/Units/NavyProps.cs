using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NavyProps : MonoBehaviour
{
    public Sprite[] sprites;

    public NationProps nation;

    public float speed;
    public bool isInBattle = false;

    private void Start()
    {
        speed = 2;
    }

    public List<TileProps> GetNeighbors() //I might just get the neighbors of the parent tile instead of doing this bullshit
    {
        List<Vector2> Coords = new List<Vector2> //get coords of neighbors
        {
            new Vector2(transform.position.x + 1, transform.position.y),
            new Vector2(transform.position.x - 1, transform.position.y),
            new Vector2(transform.position.x + 0.5f, transform.position.y + 0.86f),
            new Vector2(transform.position.x - 0.5f, transform.position.y + 0.86f),
            new Vector2(transform.position.x + 0.5f, transform.position.y - 0.86f),
            new Vector2(transform.position.x - 0.5f, transform.position.y - 0.86f)
        };

        List<TileProps> neighbors = new List<TileProps>(); //list for tiles

        int tileLayer = LayerMask.GetMask("Tiles"); //HOLY FUCK, FUCK UNITY FUCK LAYERS FUCK EVERYTHING
        foreach (Vector2 neighborCoord in Coords) //get tiles from coords
        {

            RaycastHit2D hit = Physics2D.Raycast(neighborCoord, Vector2.zero, 0, tileLayer);
            if (hit.collider != null)
            {
                TileProps neighborTile = hit.collider.GetComponent<TileProps>();
                neighbors.Add(neighborTile);
            }
        }
        return neighbors;
    }

    public void SwitchSprite(int i)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[i];
    }

    //I should probably write a method for reinforcements too 

    public void DeleteNavy()
    {
        GameState gameState = FindObjectOfType<GameState>();
        UpdateManager updateManager = FindObjectOfType<UpdateManager>();
        NavyTracker navyTracker = FindObjectOfType<NavyTracker>();

        if (gameState.activeNavy == this)
        {
            gameState.activeNavy = null;
        }

        updateManager.navies.Remove(this); //This is a bit weird. But I don't want to reference update manager in this script directly so this works for now.

        navyTracker.RemoveNavy(this);

        nation.navies.Remove(this);
        Destroy(gameObject);
    }
}