using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileInteractions : MonoBehaviour
{
    public TileProps tileProps;
    public MainUI mainUI; //for recruitment numbers input
    public TileUI tileUI;
    public GameState gameState;
    public GameObject Army;

    public static event Action<ArmyProps> ArmyRecruited; //for update manager

    private void Start()
    {
        mainUI = FindObjectOfType<MainUI>();
        tileUI = FindObjectOfType<TileUI>();
        gameState = FindObjectOfType<GameState>();
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) //apparently this is bad I don't know why
        {
            if (Input.GetMouseButtonUp(0)) //Open UI
            {
                if (gameState.gameMode == GameState.Mode.freeMode && tileProps.type != 1 && !tileProps.FOW.activeSelf)
                {
                    gameState.SelectTile(tileProps);
                   
                    if (gameState.activeArmy != null)
                    {
                        gameState.DeselectArmy(gameState.activeArmy);
                    }
                    
                    tileUI.OpenTileUI();
                    tileUI.UpdateTileUI();
                }

                else if (gameState.gameMode == GameState.Mode.recruitModeArmy && tileProps.nation != null && tileProps.nation == gameState.playerNation)
                {
                    RecruitArmy();
                }
                
                else if (gameState.gameMode == GameState.Mode.recruitModeTiles == true && tileProps.nation == gameState.activeArmy.nation && tileProps.nation == gameState.playerNation)
                {
                    AssignRecruitTiles();
                }
            }

            else if (Input.GetMouseButtonUp(1)) //MOVE ARMY
            {
                if (gameState.activeArmy != null && gameState.gameMode == GameState.Mode.freeMode)
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 targetPos = new Vector2(mousePosition.x, mousePosition.y);

                    GameObject army = gameState.activeArmy.gameObject;
                    ArmyMovement armyMovement = army.GetComponent<ArmyMovement>();
                    
                    armyMovement.progress = 0;
                    armyMovement.currentNode = 0;

                    RaycastHit2D hit = Physics2D.Raycast(targetPos, targetPos, 0, LayerMask.GetMask("Tiles"));
                    //Debug.DrawRay(army.transform.position, targetPos * 10, Color.red, 0.5f);
                    if (hit)
                    {
                        armyMovement.path = GameObject.Find("Main Camera").GetComponent<PathFinding>().GetPath(army.transform.position, hit.collider.gameObject.transform.position, 9); //not a very good line tbh can probably be simplified + also move everything from camera to controler
                    }
                }
            }
        }
    }

    // Not used for now

    //private void OnTriggerEnter2D(Collider2D collision) 
    //{
    //    NationProps collidingNation = collision.gameObject.GetComponent<ArmyProps>().nation;
        
    //    if (collidingNation != null && collidingNation != tileProps.nation)
    //    { 
    //        GetConquered(collidingNation); 
    //    }
    //}

    public void RecruitArmy()
    {
        Vector3 spawnPosition = transform.position;
        GameObject newArmy = Instantiate(Army, spawnPosition, Quaternion.identity);
        ArmyProps armyProps = newArmy.GetComponent<ArmyProps>();

        gameState.activeArmy = armyProps;
        
        tileProps.nation.armies.Add(armyProps);
        ArmyRecruited?.Invoke(armyProps);

        armyProps.nation = tileProps.nation;
        armyProps.reinforceTiles.Add(tileProps);

        mainUI.armyInfantrySizeInput.gameObject.SetActive(true);
        mainUI.armyCavalrySizeInput.gameObject.SetActive(true);
        mainUI.armySizeDoneButton.gameObject.SetActive(true);
        mainUI.recruitmentCancelButton.gameObject.SetActive(true);

        tileProps.isReinforceTile = true;

        gameState.gameMode = GameState.Mode.recruitModeTiles;
    }

    public void AssignRecruitTiles()
    {
        gameState.activeArmy.reinforceTiles.Add(tileProps);
        tileProps.isReinforceTile = true;
    }

    // REMOVING THIS FOR NOW
    //public void GetConquered(NationProps newNation) 
    //{
    //    if (tileProps.nation != null)
    //    {
    //        tileProps.nation.GetComponent<NationProps>().tiles.Remove(tileProps);
    //    }
    //    newNation.GetComponent<NationProps>().tiles.Add(tileProps);
    //    tileProps.nation = newNation;
    //}
}
