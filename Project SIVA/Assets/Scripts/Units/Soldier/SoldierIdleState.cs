using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierIdleState : SoldierBaseState
{
    public override void EnterState(Soldier soldier)
    {
        soldier.hitpoints = 100;
    }
    public override void UpdateState(Soldier soldier)
    {
        // Animate here
    }
    
}
