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

        // If in range of melee, melee attack
        if (inAttackRange)
        {
            //soldier.animationController.MeleeAnimation();
            Debug.Log("Melee Attack: Soldier");
            soldier.MeleeAttack();
        }
        // Otherwise aggro-move towards the player
        else
        {
            Debug.Log("Melee move: Soldier");
            soldier.AggroMove();
        }

        if (soldier.hitpoints <= 25)
        {
            soldier.SwitchState(soldier.soldierRetreatState);
        }



    }
}
