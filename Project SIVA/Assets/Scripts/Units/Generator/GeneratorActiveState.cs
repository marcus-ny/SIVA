using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorActiveState : GeneratorBaseState
{
    public override void EnterState(Generator generator)
    {
        // Play starting up animation
        // Emit light around
        generator.GetComponent<GeneratorAnimationController>().StartActive();
        generator.EmitLight(true);

        GameObject hpBar = generator.GetComponent<Transform>().GetChild(0).gameObject;
        hpBar.SetActive(true);
    }

    public override void UpdateState(Generator generator)
    {
        generator.GetComponent<GeneratorAnimationController>().StayActive();
        // If enemy is in light tiles nearby, zap him
        if (generator.PlayerDetectedInRange())
        {
            DamageManager.Instance.DealDamageToPlayer(5);
        }
        // If HP == 0, disable light tiles and go to dead state
        if (generator.hitpoints <= 0)
        {
            generator.SwitchState(generator.generatorDeadState);
        }
    }
}