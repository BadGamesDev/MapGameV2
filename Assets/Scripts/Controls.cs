using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controls : MonoBehaviour
{
    public GameState gameState;
    public ArmyUI armyUI;
    public TileUI tileUI;
    public RecruitmentManager recruitmentManager;

    public Camera mainCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // LEFT-CLICK
        {
            Debug.Log("step1");

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetPos = new Vector2(mousePosition.x, mousePosition.y);
            RaycastHit2D hit = Physics2D.Raycast(targetPos, targetPos, 0);

            if (hit)
            {
                GameObject clickedObject = hit.collider.gameObject;

                //CLICKED AN ARMY ###########################################
                if (clickedObject.CompareTag("Army"))
                {
                    ArmyProps army = clickedObject.GetComponent<ArmyProps>();
                    Debug.Log("you just clicked an army");
                    
                    if (gameState.gameMode == GameState.Mode.freeMode && gameState.playerNation == army.nation) //This is temporary, player should be able to check armies of everyone
                    {
                        if (gameState.activeArmy != null)
                        {
                            gameState.DeselectArmy(gameState.activeArmy);
                        }

                        gameState.SelectArmy(army);

                        armyUI.OpenArmyUI();
                    }

                }
                
                //CLICKED A TILE ############################################
                else if (clickedObject.CompareTag("Tile") && !EventSystem.current.IsPointerOverGameObject())
                {
                    TileProps tile = clickedObject.GetComponent<TileProps>();
                    Debug.Log("you just clicked a tile");

                    if (gameState.gameMode == GameState.Mode.freeMode && tile.type != 1 && !tile.FOW.activeSelf) //normal click, just open the UI
                    {
                        gameState.SelectTile(tile);

                        if (gameState.activeArmy != null)
                        {
                            gameState.DeselectArmy(gameState.activeArmy);
                        }

                        tileUI.OpenTileUI();
                        tileUI.UpdateTileUI();
                    }

                    else if (gameState.gameMode == GameState.Mode.recruitModeArmy && tile.nation != null && tile.nation == gameState.playerNation && tile.isReinforceTile == false) //recruit army
                    {
                        recruitmentManager.RecruitArmy(tile);
                    }

                    else if (gameState.gameMode == GameState.Mode.recruitModeTiles == true && tile.nation == gameState.activeArmy.nation && tile.nation == gameState.playerNation) //asssign tile to an army
                    {
                        recruitmentManager.AssignRecruitTiles(tile);
                    }
                }
                
                else
                {
                    Debug.Log("the fuck did you click on dawg?");
                }
            }
        }

        else if (Input.GetMouseButtonUp(1) && !EventSystem.current.IsPointerOverGameObject()) // RIGHT-CLICK
        {
            if (gameState.activeArmy != null && gameState.gameMode == GameState.Mode.freeMode) // move army
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 targetPos = new Vector2(mousePosition.x, mousePosition.y);

                GameObject army = gameState.activeArmy.gameObject;
                ArmyMovement armyMovement = army.GetComponent<ArmyMovement>();

                armyMovement.progress = 0;
                armyMovement.currentNode = 0;

                RaycastHit2D hit = Physics2D.Raycast(targetPos, targetPos, 0, LayerMask.GetMask("Tiles"));
                
                if (hit)
                {
                    armyMovement.path = GameObject.Find("Main Camera").GetComponent<PathFinding>().GetPath(army.transform.position, hit.collider.gameObject.transform.position, 9); //not a very good line tbh can probably be simplified + also move everything from camera to controler
                }
            }
        }
    }
}