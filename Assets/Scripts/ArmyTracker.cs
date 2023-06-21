using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyTracker : MonoBehaviour
{
    public static ArmyTracker instance; // Maybe I should keep doing things this way

    private Dictionary<ArmyProps, Vector2> armyPositions = new Dictionary<ArmyProps, Vector2>();

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

    public void UpdateArmyPosition(ArmyProps army, Vector2 newPosition)
    {
        if (armyPositions.ContainsKey(army))
        {
            armyPositions[army] = newPosition;

            if (CheckArmyCollision(army, newPosition))
            {
                ArmyProps enemyArmy = GetCollidingArmy(army, newPosition);
                if (enemyArmy != null && enemyArmy.nation != army.nation)
                {
                    InitiateCombat(army, enemyArmy);
                }
            }
        }
    }

    public Vector2 GetArmyPosition(ArmyProps army)
    {
        if (armyPositions.ContainsKey(army))
        {
            return armyPositions[army];
        }

        return Vector2.zero;
    }

    public void RemoveArmy(ArmyProps army)
    {
        if (armyPositions.ContainsKey(army))
        {
            armyPositions.Remove(army);
        }
    }

    private bool CheckArmyCollision(ArmyProps army, Vector2 position)
    {
        foreach (KeyValuePair<ArmyProps, Vector2> pair in armyPositions)
        {
            if (pair.Key != army && pair.Value == position)
            {
                return true;
            }
        }
        return false;
    }

    private ArmyProps GetCollidingArmy(ArmyProps army, Vector2 position)
    {
        foreach (KeyValuePair<ArmyProps, Vector2> pair in armyPositions)
        {
            if (pair.Key != army && pair.Value == position)
            {
                return pair.Key;
            }
        }
        return null;
    }

    private void InitiateCombat(ArmyProps army1, ArmyProps army2)
    {
        Debug.Log("Combat initiated between " + army1.name + " and " + army2.name);
    }

    private void OnDrawGizmos() //debug
    {
        Gizmos.color = Color.green;
        foreach (KeyValuePair<ArmyProps, Vector2> entry in armyPositions)
        {
            Gizmos.DrawSphere(entry.Value, 0.5f);
        }
    }
}