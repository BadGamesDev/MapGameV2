using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleProps : MonoBehaviour
{
    public List<ArmyProps> armies;

    public NationProps attacker;
    public NationProps defender;

    public List<ArmyProps> attackerArmies;
    public List<ArmyProps> defenderArmies;

    public TMP_Text attackerSizeText;
    public TMP_Text defenderSizeText;

    public void EndBattle()
    {
        foreach (ArmyProps army in attackerArmies)
        {
            army.isInBattle = false;
            army.armySizeText.gameObject.SetActive(true);
            army.armySizeText.text = army.curSize.ToString(); //THE AMOUNT OF BULSHITTERY I DO IN ORDER TO GET A WORKING DEMO IS UNREAL!
            army.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y);
            
        }
        foreach (ArmyProps army in defenderArmies)
        {
            army.isInBattle = false;
            army.armySizeText.gameObject.SetActive(true);
            army.armySizeText.text = army.curSize.ToString();
            army.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y);
        }
        ArmyTracker armyTracker = FindObjectOfType<ArmyTracker>();
        armyTracker.battlePositions.Remove(this);

        Destroy(gameObject);
    }

    public void UpdateText()
    {
        float attackerSize = 0;
        float defenderSize = 0;
        
        foreach(ArmyProps army in attackerArmies)
        {
            attackerSize += army.curSize;
        }

        foreach (ArmyProps army in defenderArmies)
        {
            defenderSize += army.curSize;
        }

        attackerSizeText.text = attackerSize.ToString();
        defenderSizeText.text = defenderSize.ToString();
    }
}
