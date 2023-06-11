using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorHiddenState : GeneratorBaseState
{
    public override void EnterState(Generator generator)
    {
        // Stay hidden
        generator.hitpoints = generator.maxHp;
    }

    public override void UpdateState(Generator generator)
    {       
        // If enemy detected within a range, change state to active
        if (generator.PlayerDetectedInRange())
        {            
            generator.SwitchState(generator.generatorActiveState);
        }
    }
}
