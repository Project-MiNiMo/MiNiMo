public class MinimoWorkState : StateBase<Minimo>
{
    public MinimoWorkState(Minimo owner) : base(owner) { }

    public override void Enter()
    {
        _owner.SetAnimation("Work");
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {

    }
}
