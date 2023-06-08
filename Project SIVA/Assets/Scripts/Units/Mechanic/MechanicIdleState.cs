using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicIdleState : MechanicBaseState
{
    public override void EnterState(Mechanic mechanic)
    {
        Debug.Log("mechanic is idle now");
    }

    public override void UpdateState(Mechanic mechanic)
    {
        mechanic.actionsPerformed += 2;
    }
}
