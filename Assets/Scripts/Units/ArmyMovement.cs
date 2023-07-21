using System.Collections.Generic;
using UnityEngine;

public class ArmyMovement : MonoBehaviour
{
    public ArmyTracker armyTracker;
    public ArmyProps armyProps;
    public TimeManager timeManager;
    public MapGenerator mapGenerator; //I REALLY don't like referencing this shit everywhere

    public List<Vector2> path = new List<Vector2>();

    public int currentNode = 0;
    public float progress = 0;

    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        armyTracker = FindObjectOfType<ArmyTracker>();
        armyTracker.AddArmy(armyProps, transform.position);
    }

    public void MarchArmy() //called in updateManager everyday
    {
        if (path.Count > 0 && armyProps.isInBattle == false)
        {
            if (progress >= 5)
            {
                int tileLayer = LayerMask.GetMask("Tiles");
                RaycastHit2D hit = Physics2D.Raycast(path[currentNode+1], path[currentNode+1], 0, tileLayer); //Index out of range bug, happens when on the left or right side of water and you click on water
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
                    currentNode = 0;
                    progress = 5;
                }
            }
            else
            {
                progress += armyProps.speed;
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

        if(tile.GetComponent<TileProps>().nation == null)
        { 
            int ambushRoll = tile.GetComponent<TileProps>().nativeAgressiveness + Random.Range(0, 100);
            if (ambushRoll >= 100)
            {
                RecruitmentManager recruitmentManager = FindObjectOfType<RecruitmentManager>();
                GameObject nativeArmyPrefab = Instantiate(recruitmentManager.armyPrefab, tile.transform.position, Quaternion.identity);
                ArmyProps nativeArmy = nativeArmyPrefab.GetComponent<ArmyProps>();

                armyTracker.AddArmy(nativeArmy, nativeArmy.transform.position);

                nativeArmy.SwitchSprite(2);
                nativeArmy.curInfantry = 50;
                nativeArmy.curCavalry = 50;
                nativeArmy.nation = GameObject.Find("Nation1").GetComponent<NationProps>();
                //nativeArmy.isInBattle = true; //armytracker is not working so I have to do this shit manually

                Debug.Log("AMBUSH!");
            }
        }

        armyTracker.UpdateArmyPosition(armyProps, transform.position);
    }
}