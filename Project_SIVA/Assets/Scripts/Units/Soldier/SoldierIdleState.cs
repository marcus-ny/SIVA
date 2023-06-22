using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierIdleState : SoldierBaseState
{
    public override void EnterState(Soldier soldier)
    {
        soldier.hitpoints = soldier.maxHp;
    }
    public override void UpdateState(Soldier soldier)
    {
        // Animate here
        // if enemy detected


        if (soldier.hitpoints <= 25)
        {
            soldier.SwitchState(soldier.soldierRetreatState);
        }
        soldier.SwitchState(soldier.soldierAggroState);
    }
    
}
