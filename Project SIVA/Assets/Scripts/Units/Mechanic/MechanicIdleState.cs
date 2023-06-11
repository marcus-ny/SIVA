using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicIdleState : MechanicBaseState
{
    public override void EnterState(Mechanic mechanic)
    {
        Debug.Log("Mechanic is in idle state");
        mechanic.hitpoints = mechanic.maxHp;
    }

    public override void UpdateState(Mechanic mechanic)
    {
        if (EnemyManager.Instance.GetLowestHpAlly().hpRatio == 1)
        {
            mechanic.actionsPerformed += 2;
        } else
        {
            mechanic.SwitchState(mechanic.mechanicHealState);
        }
        
    }
}
