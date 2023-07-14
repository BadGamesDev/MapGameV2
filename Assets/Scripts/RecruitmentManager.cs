using UnityEngine;

public class RecruitmentManager : MonoBehaviour
{
    public GameState gameState;
    public MainUI mainUI;
    public ArmyUI armyUI;
    public MapModes mapModes; //ATROCIOUS SOLUTION! FIND ANOTHER WAY!


    public void RecruitArmyButton()
    {
        if (gameState.gameMode != GameState.Mode.recruitModeArmy)
        {
            gameState.activeArmy = null;
            gameState.gameMode = GameState.Mode.recruitModeArmy;

            if (gameState.activeArmy != null)
            {
                gameState.activeArmy = null;
                gameState.activeArmy.SwitchSprite(0);
            }
        }

        else
        {
            mapModes.mapMode = MapModes.Mode.political; //It should probably be back to the map mode you have choosen but come on dude... we can do that later
            gameState.gameMode = GameState.Mode.freeMode;
        }
    }

    public void AssignTroopValues()
    {
        int infantryCount = int.Parse(mainUI.armyInfantrySizeInput.text);
        int cavalryCount = int.Parse(mainUI.armyCavalrySizeInput.text);

        gameState.activeArmy.maxInfantry = infantryCount;
        gameState.activeArmy.maxCavalry = cavalryCount;
        gameState.activeArmy.maxSize = infantryCount + cavalryCount;

        mainUI.armyInfantrySizeInput.gameObject.SetActive(false);
        mainUI.armyCavalrySizeInput.gameObject.SetActive(false);
        mainUI.armySizeDoneButton.gameObject.SetActive(false);

        gameState.activeArmy = null;

        mapModes.mapMode = MapModes.Mode.political;
        gameState.gameMode = GameState.Mode.freeMode;
    }

    public void CancelRecruitment()
    {
        armyUI.DeleteArmy(gameState.activeArmy);

        mainUI.armyInfantrySizeInput.gameObject.SetActive(false);
        mainUI.armyCavalrySizeInput.gameObject.SetActive(false);
        mainUI.armySizeDoneButton.gameObject.SetActive(false);
        mainUI.recruitmentCancelButton.gameObject.SetActive(false);


        mapModes.mapMode = MapModes.Mode.political;
        gameState.gameMode = GameState.Mode.freeMode;
    }
}