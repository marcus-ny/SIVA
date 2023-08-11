public class GeneratorDeadState : GeneratorBaseState
{
    public override void EnterState(Generator generator)
    {
        // Remove the generator from game world
        // Use either destroy or disable
    }

    public override void UpdateState(Generator generator)
    {
        throw new System.NotImplementedException();
    }
}