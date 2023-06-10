using System.Collections.Generic;
using UnityEngine;

public class RecruitmentManager : MonoBehaviour
{
    public GameState gameState;
  
    public void RecruitArmy()    
    {
        gameState.recruitModeArmy = true;
    }
}