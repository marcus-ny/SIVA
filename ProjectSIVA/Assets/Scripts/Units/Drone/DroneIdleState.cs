using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneIdleState : DroneBaseState
{
    public override void EnterState(Drone drone)
    {
        drone.hitpoints = drone.maxHp;
        drone.EmitLight(true);
    }

    public override void UpdateState(Drone drone)
    {
        if (drone.player != null)
        {
            drone.SwitchState(drone.droneSearchState);
        }
    }
}
