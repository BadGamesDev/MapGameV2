using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileInteractions : MonoBehaviour
{
    public TileProps tileProps;
    public MainUI mainUI;
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
        if (Input.GetMouseButtonUp(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) //APPARENTLY THIS IS ACTUALLY BAD BUT I DON'T KNOW WHY
            {
                if (gameState.gameMode == GameState.Mode.freeMode && tileProps.type != 1)
                {
                    gameState.activeTile = tileProps;

                    if (tileProps.nation == gameState.playerNation)
                    {
                        tileUI.OpenTileUI();
                        tileUI.UpdateTileUI();
                    }
                    else
                    {
                        tileUI.OpenTileUI();
                        tileUI.UpdateTileUI();
                    }
                }

                else if (gameState.gameMode == GameState.Mode.recruitModeArmy && tileProps.nation != null)
                {
                    RecruitArmy();
                }
                else if (gameState.gameMode == GameState.Mode.recruitModeTiles == true && tileProps.nation == gameState.activeArmy.nation)
                {
                    AssignRecruitTiles();
                }
            }
        }

        else if (Input.GetMouseButtonUp(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) //SAME PROBLEM AS STATED ABOVE
            {
                if (gameState.activeArmy != null && gameState.gameMode == GameState.Mode.freeMode)
                {
                    Debug.Log("Mobe");
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 pos = new Vector2(mousePosition.x, mousePosition.y);

                    GameObject army = gameState.activeArmy.gameObject;
                    RaycastHit2D hit = Physics2D.Raycast(pos, pos, 0, LayerMask.GetMask("Tiles"));
                    if (hit)
                    {
                        army.GetComponent<ArmyMovement>().path = new List<Vector2>();
                        army.GetComponent<ArmyMovement>().currNode = 0;
                        army.GetComponent<ArmyMovement>().delay = 0.5f;
                        army.GetComponent<ArmyMovement>().path = GameObject.Find("Main Camera").GetComponent<PathFinding>().GetPath(army.transform.position, hit.collider.gameObject.transform.position, 9); //not a very good line tbh + Move everything from camera to controler
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NationProps collidingNation = collision.gameObject.GetComponent<ArmyProps>().nation;
        
        if (collidingNation != null && collidingNation != tileProps.nation)
        { 
            GetConquered(collidingNation); 
        }
    }

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

        tileProps.isReinforceTile = true;

        gameState.gameMode = GameState.Mode.recruitModeTiles;
    }

    public void AssignRecruitTiles()
    {
        gameState.activeArmy.reinforceTiles.Add(tileProps);
        tileProps.isReinforceTile = true;
    }

    public void GetConquered(NationProps newNation)
    {
        if (tileProps.nation != null)
        {
            tileProps.nation.GetComponent<NationProps>().tiles.Remove(tileProps);
        }
        newNation.GetComponent<NationProps>().tiles.Add(tileProps);
        tileProps.nation = newNation;
    }
}
