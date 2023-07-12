using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;

public class ArmyTracker : MonoBehaviour
{
    public static ArmyTracker instance; // Maybe I should keep doing things this way
    public GameObject battlePrefab;

    public Dictionary<ArmyProps, Vector2> armyPositions = new Dictionary<ArmyProps, Vector2>();
    public Dictionary<BattleProps, Vector2> battlePositions = new Dictionary<BattleProps, Vector2>();

    private void Awake()
    {
        // Set up the singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddArmy(ArmyProps army, Vector2 position)
    {
        if (!armyPositions.ContainsKey(army))
        {
            armyPositions.Add(army, position);
        }
    }

    public void RemoveArmy(ArmyProps army)
    {
        if (armyPositions.ContainsKey(army))
        {
            armyPositions.Remove(army);
        }
    }

    public void UpdateArmyPosition(ArmyProps army, Vector2 newPosition) //I can optimise the fuck out of this shit
    {
        if (armyPositions.ContainsKey(army))
        {
            armyPositions[army] = newPosition;
            bool battleFound = false;

            foreach (KeyValuePair<BattleProps, Vector2> pair in battlePositions)
            {
                if (pair.Key.attacker == army.nation || pair.Key.defender == army.nation && pair.Value == newPosition)
                {
                    ReinforceBattle(army, pair.Key);
                    battleFound = true;
                }
            }

            if (battleFound == false)
            {
                foreach (KeyValuePair<ArmyProps, Vector2> pair in armyPositions)
                {
                    if (pair.Value == newPosition && pair.Key.nation != army.nation)
                    {
                        InitiateBattle(army, pair.Key, newPosition);
                    }
                }
            }
        }
    }

    private void InitiateBattle(ArmyProps attacker, ArmyProps defender, Vector2 position)
    {
        Debug.Log("Combat initiated between " + attacker.name + " and " + defender.name);
        GameObject prefab = Instantiate(battlePrefab, position, Quaternion.identity);
        BattleProps battle = battlePrefab.GetComponent<BattleProps>();

        battle.attacker = attacker.nation;
        battle.attacker = defender.nation;

        battle.attackerArmies.Add(attacker);
        battle.defenderArmies.Add(defender);

        battlePositions.Add(battle ,battle.transform.position);
    }

    private void ReinforceBattle(ArmyProps army, BattleProps battle) 
    {

        Debug.Log("Combat reinforced on the side of" + army.nation);
        army.isInBattle = true;
        
        if(army.nation = battle.attacker)
        {
            battle.attackerArmies.Add(army);
        }
        
        if (army.nation = battle.defender)
        {
            battle.defenderArmies.Add(army);
        }
    }
}