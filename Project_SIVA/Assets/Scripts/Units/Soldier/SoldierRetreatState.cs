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
        if (EnemyManager.Instance.FindMechanicLocations().Count > 0)
        {
            soldier.RetreatMove();
            if (soldier.hitpoints >= 50)
            {
                soldier.SwitchState(soldier.soldierAggroState);
            }
        } else
        {
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
}
