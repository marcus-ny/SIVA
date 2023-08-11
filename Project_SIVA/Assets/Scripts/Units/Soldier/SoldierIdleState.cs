using UnityEngine;

public class SoldierIdleState : SoldierBaseState
{
    public override void EnterState(Soldier soldier)
    {
        soldier.hitpoints = soldier.maxHp;
    }
    public override void UpdateState(Soldier soldier)
    {
        if (soldier.hitpoints <= 25)
        {
            soldier.SwitchState(soldier.soldierRetreatState);
        }

        // HP is high enough to continue battle
        Vector3Int soldierLocation = soldier.activeTile.gridLocation;
        Vector3Int playerLocation = soldier.player.activeTile.gridLocation;


        bool inAggroRange = (Mathf.Abs(soldierLocation.x - playerLocation.x) < 4)
        && (Mathf.Abs(soldierLocation.y - playerLocation.y) < 4);

        if (inAggroRange)
        {
            soldier.SwitchState(soldier.soldierAggroState);
        }
        else
        {
            soldier.SwitchState(soldier.soldierRangeState);
        }
    }

}
