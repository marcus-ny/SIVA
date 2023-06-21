using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DroneBaseState
{
    public abstract void EnterState(Drone drone);

    public abstract void UpdateState(Drone drone);
}
