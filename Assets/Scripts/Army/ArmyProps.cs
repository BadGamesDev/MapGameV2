using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmyProps : MonoBehaviour
{
    public Sprite[] sprites;

    public NationProps nation;
    public List<TileProps> reinforceTiles = new List<TileProps>();

    public TMP_Text armySizeText;

    public bool reinforce;
    public bool isInBattle; // this is unused for now 

    public int maxSize;
    public int curSize;
    public int availablePop;

    public int maxInfantry;
    public int maxCavalry;
    
    public int curInfantry;
    public int curCavalry;

    private void Start()
    {
        reinforce = true;
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

    public void TakeLosses(int i)
    {
        curCavalry -= i;
        curInfantry -= i;
    }
}
