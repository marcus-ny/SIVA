using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAggroState : SoldierBaseState
{
    public override void EnterState(Soldier soldier)
    {
        Debug.Log("Soldier enters aggro state");
    }
    public override void UpdateState(Soldier soldier)
    {
        
        // Otherwise, carry out an aggressive sequence of actions
        Vector3Int soldierLocation = soldier.activeTile.gridLocation;
        Vector3Int playerLocation = soldier.player.activeTile.gridLocation;

        bool inAttackRange = (Mathf.Abs(soldierLocation.x - playerLocation.x) < 2)
        && (Mathf.Abs(soldierLocation.y - playerLocation.y) < 2);

        bool inAggroRange = (Mathf.Abs(soldierLocation.x - playerLocation.x) < 4)
        && (Mathf.Abs(soldierLocation.y - playerLocation.y) < 4);

        if (!inAggroRange)
        {
            soldier.TakePositionForRange();
            soldier.RangeAttack();
        }
        // If in range of melee, melee attack
        else if (inAttackRange)
        {
            //soldier.animationController.MeleeAnimation();
            soldier.MeleeAttack();
        }
        // Otherwise aggro-move towards the player
        else
        {
            soldier.AggroMove();
        }


        // Note to self: Should this be at the top or bottom of the update method
        // In general, should state changes be accounted for after the actions or before?
        // If HP is low after carrying out all these actions, change the state in preparation
        // for the next turn
        
        if (soldier.hitpoints <= 25)
        {
            soldier.SwitchState(soldier.soldierRetreatState);
        }
        

        
    }
}