public class MechanicIdleState : MechanicBaseState
{
    public override void EnterState(Mechanic mechanic)
    {

        mechanic.hitpoints = mechanic.maxHp;
    }

    public override void UpdateState(Mechanic mechanic)
    {
        if (EnemyManager.Instance.GetLowestHpAlly().hpRatio == 1)
        {
            mechanic.actionsPerformed += 2;
        }
        else
        {
            mechanic.SwitchState(mechanic.mechanicHealState);
        }

    }
}
