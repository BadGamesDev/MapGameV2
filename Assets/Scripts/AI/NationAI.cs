using System;
using System.Linq;
using UnityEngine;
using static TimeManager; //I REALLY DON'T LIKE THIS

public class NationAI : MonoBehaviour
{
    public NationProps nationProps;
    public GameObject Army;
    public static event Action<ArmyProps> ArmyRecruitedAI; //for Update manager (am I doing this shit right? no idea...)

    private void Start()
    {
        monthTickSend += OnMonthTick;
    }

    public void OnMonthTick()
    {
        RecruitArmyAI();
    }

    public void RecruitArmyAI()
    {
        if(nationProps.income > nationProps.expense * 1.5f + 10 && nationProps.armies.Sum(army => army.maxInfantry) <= nationProps.income * 2) //Formula works for now but change it in the future
        {
            int randomIndex;
            TileProps randomTile;
            bool foundValidTile = false;

            int tryCount = 0;
            while (!foundValidTile && tryCount < 100)
            {
                tryCount++;
                randomIndex = UnityEngine.Random.Range(0, nationProps.tiles.Count);
                randomTile = nationProps.tiles[randomIndex];

                if (!randomTile.isReinforceTile) //check if already reinforce tile
                {
                    foundValidTile = true;

                    Vector3 spawnPosition = randomTile.transform.position;
                    GameObject newArmy = Instantiate(Army, spawnPosition, Quaternion.identity);
                    ArmyProps armyProps = newArmy.GetComponent<ArmyProps>();

                    randomTile.nation.armies.Add(armyProps);
                    ArmyRecruitedAI?.Invoke(armyProps); //NOT WORKING

                    armyProps.nation = randomTile.nation;
                    armyProps.reinforceTiles.Add(randomTile);

                    armyProps.maxInfantry = 1000; //recruit function AI addition

                    armyProps.desiredSize = armyProps.maxInfantry + armyProps.maxCavalry;

                    randomTile.isReinforceTile = true;
                }
            }
        }
    }

    public void MoveArmyAI()
    {
        foreach (ArmyProps army in nationProps.armies)
        {
            //if (army.targetTile == null)
            {
                TileProps targetTile = FindTargetTile(army);
                if (targetTile != null)
                {
              //      MoveArmyToTile(army, targetTile);
                }
            }
        }
    }

    private TileProps FindTargetTile(ArmyProps army)
    {
        int randomIndex = UnityEngine.Random.Range(0, nationProps.tiles.Count);
        return nationProps.tiles[randomIndex];
    }

    public void ColonizeTile()
    {

    }
}
