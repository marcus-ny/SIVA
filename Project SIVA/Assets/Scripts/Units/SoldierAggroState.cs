using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAggroState : SoldierBaseState
{
    public override void EnterState(Soldier soldier)
    {
        
    }
    public override void UpdateState(Soldier soldier)
    {

        // Otherwise, carry out an aggressive sequence of actions
        Vector3Int soldierLocation = soldier.activeTile.gridLocation;
        Vector3Int playerLocation = soldier.player.activeTile.gridLocation;

        bool inAttackRange = (Mathf.Abs(soldierLocation.x - playerLocation.x) < 2)
        && (Mathf.Abs(soldierLocation.y - playerLocation.y) < 2);

        // If in range of melee, melee attack
        if (inAttackRange)
        {
            soldier.MeleeAttack();
        }
        // Otherwise aggro-move towards the player
        else
        {
            soldier.AggroMove();
        }


        // If HP is low after carrying out all these actions, change the state in preparation
        // for the next turn
        if (soldier.hitpoints < 25)
        {
            soldier.SwitchState(soldier.soldierRetreatState);
        }


        
    }
}
