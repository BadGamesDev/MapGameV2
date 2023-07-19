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
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // LEFT-CLICK ##############################3
        {
            Debug.Log("step1");

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetPos = new Vector2(mousePosition.x, mousePosition.y);
            RaycastHit2D hit = Physics2D.Raycast(targetPos, targetPos, 0);

            if (hit)
            {
                GameObject clickedObject = hit.collider.gameObject;

                //LEFT CLICKED A NAVY ############################################
                if (clickedObject.CompareTag("Navy"))
                {
                    NavyProps navy = clickedObject.GetComponent<NavyProps>();
                    Debug.Log("you just clicked a navy");

                    if (gameState.gameMode == GameState.Mode.freeMode && gameState.playerNation == navy.nation)
                    {
                        if (gameState.activeTile != null)
                        {
                            gameState.DeselectTile(gameState.activeTile);
                        }
                        if (gameState.activeArmy != null)
                        {
                            gameState.DeselectArmy(gameState.activeArmy);
                        }
                        if (gameState.activeNavy != null)
                        {
                            gameState.DeselectNavy(gameState.activeNavy);
                        }

                        gameState.SelectNavy(navy);

                        //navyUI.OpenNavyUI();
                    }

                }

                //LEFT CLICKED AN ARMY ###########################################
                else if (clickedObject.CompareTag("Army"))
                {
                    ArmyProps army = clickedObject.GetComponent<ArmyProps>();
                    Debug.Log("you just clicked an army");
                    
                    if (gameState.gameMode == GameState.Mode.freeMode && gameState.playerNation == army.nation) //This is temporary, player should be able to check armies of everyone
                    {
                        if (gameState.activeTile != null)
                        {
                            gameState.DeselectTile(gameState.activeTile);
                        }
                        if (gameState.activeArmy != null)
                        {
                            gameState.DeselectArmy(gameState.activeArmy);
                        }
                        if (gameState.activeNavy != null)
                        {
                            gameState.DeselectNavy(gameState.activeNavy);
                        }

                        gameState.SelectArmy(army);

                        armyUI.OpenArmyUI();
                    }

                }
                
                //LEFT CLICKED A TILE ############################################
                else if (clickedObject.CompareTag("Tile") && !EventSystem.current.IsPointerOverGameObject())
                {
                    TileProps tile = clickedObject.GetComponent<TileProps>();
                    Debug.Log("you just clicked a tile");

                    if (gameState.gameMode == GameState.Mode.freeMode && !tile.FOW.activeSelf) //normal click, just open the UI
                    {
                        gameState.SelectTile(tile);
                        
                        if(tile.type != 1)
                        {
                            tileUI.OpenTileUI();
                            tileUI.UpdateTileUI();
                        }

                        if (gameState.activeTile != null)
                        {
                            gameState.DeselectTile(gameState.activeTile);
                        }
                        if (gameState.activeArmy != null)
                        {
                            gameState.DeselectArmy(gameState.activeArmy);
                        }
                        if (gameState.activeNavy != null)
                        {
                            gameState.DeselectNavy(gameState.activeNavy);
                        }
                        
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

        else if (Input.GetMouseButtonUp(1) && !EventSystem.current.IsPointerOverGameObject()) // RIGHT-CLICK #####################################3
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
            
            else if (gameState.activeNavy != null && gameState.gameMode == GameState.Mode.freeMode) // move navy
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 targetPos = new Vector2(mousePosition.x, mousePosition.y);

                GameObject navy = gameState.activeNavy.gameObject;
                NavyMovement navyMovement = navy.GetComponent<NavyMovement>();

                navyMovement.progress = 0;
                navyMovement.currentNode = 0;

                RaycastHit2D hit = Physics2D.Raycast(targetPos, targetPos, 0, LayerMask.GetMask("Tiles"));

                if (hit)
                {
                    navyMovement.path = GameObject.Find("Main Camera").GetComponent<PathFinding>().GetPath(navy.transform.position, hit.collider.gameObject.transform.position, 9999999); //not a very good line tbh can probably be simplified + also move everything from camera to controler
                }
            }
        }
    }
}