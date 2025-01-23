public class MinimoWorkState : StateBase<Minimo>
{
    public MinimoWorkState(Minimo owner) : base(owner) { }

    public override void Enter()
    {
        _owner.SetAnimation("IsWork", true);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {

    }
}
