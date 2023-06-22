using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratorBaseState
{
    public abstract void EnterState(Generator generator);

    public abstract void UpdateState(Generator generator);
}
