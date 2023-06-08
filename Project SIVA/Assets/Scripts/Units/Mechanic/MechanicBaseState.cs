using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MechanicBaseState
{
    public abstract void EnterState(Mechanic mechanic);

    public abstract void UpdateState(Mechanic mechanic);
}
