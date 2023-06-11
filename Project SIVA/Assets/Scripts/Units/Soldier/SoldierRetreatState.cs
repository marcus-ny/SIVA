using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierRetreatState : SoldierBaseState
{
    public override void EnterState(Soldier soldier)
    {
        // Set the weak animation here
        Debug.Log("Soldier enters retreat state");
    }
    public override void UpdateState(Soldier soldier)
    {
        soldier.RetreatMove();

        if (soldier.hitpoints >= 50)
        {
            soldier.SwitchState(soldier.soldierAggroState);
        }
        
    }
}
