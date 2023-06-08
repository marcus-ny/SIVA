using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoldierBaseState
{
    public abstract void EnterState(Soldier soldier);

    public abstract void UpdateState(Soldier soldier);
}
