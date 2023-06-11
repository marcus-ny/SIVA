using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicHealState : MechanicBaseState
{
    public override void EnterState(Mechanic mechanic)
    {
        Debug.Log("Mechanic enters heal state");
    }

    public override void UpdateState(Mechanic mechanic)
    {
        Vector3Int mechanicLocation = mechanic.activeTile.gridLocation;
        
        mechanic.allyLowHp = EnemyManager.Instance.GetLowestHpAlly();

        if (mechanic.allyLowHp.hpRatio == 1)
        {
            mechanic.SwitchState(mechanic.mechanicIdleState);
        }
        Vector3Int allyLocation = mechanic.allyLowHp.activeTile.gridLocation;

        bool inHealRange = (Mathf.Abs(mechanicLocation.x - allyLocation.x) < 2)
        && (Mathf.Abs(mechanicLocation.y - allyLocation.y) < 2);

        if (inHealRange)
        {
            mechanic.Heal();
        } else
        {
            mechanic.MoveToAlly();
        }


        // Add State Transition here later
    }
}
