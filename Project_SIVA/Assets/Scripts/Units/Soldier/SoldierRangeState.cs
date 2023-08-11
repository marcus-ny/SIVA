using UnityEngine;

public class SoldierRangeState : SoldierBaseState
{
    public override void EnterState(Soldier soldier)
    {

    }
    public override void UpdateState(Soldier soldier)
    {
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
        else
        {
            soldier.SwitchState(soldier.soldierAggroState);
        }

        if (soldier.hitpoints <= 25)
        {
            soldier.SwitchState(soldier.soldierRetreatState);
        }
    }
}
