using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSearchState : DroneBaseState
{
    public override void EnterState(Drone drone)
    {

    }

    public override void UpdateState(Drone drone)
    {
        //drone.EmitLight(false);
        drone.Search();
        //drone.EmitLight(true);
    }
}
