using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleProps : MonoBehaviour
{
    public List<ArmyProps> armies;

    public NationProps attacker;
    public NationProps defender;

    public List<ArmyProps> attackerArmies;
    public List<ArmyProps> defenderArmies;
}
