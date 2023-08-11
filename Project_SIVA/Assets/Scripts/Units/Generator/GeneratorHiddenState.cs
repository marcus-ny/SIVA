using UnityEngine;

public class GeneratorHiddenState : GeneratorBaseState
{
    public override void EnterState(Generator generator)
    {
        // Stay hidden
        generator.hitpoints = generator.maxHp;
        GameObject hpBar = generator.GetComponent<Transform>().GetChild(0).gameObject;
        hpBar.SetActive(false);
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
