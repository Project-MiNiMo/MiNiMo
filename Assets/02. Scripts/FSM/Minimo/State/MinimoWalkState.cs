public class MinimoWalkState : StateBase<Minimo>
{
    public MinimoWalkState(Minimo owner) : base(owner) { }

    public override void Enter()
    {
        _owner.SetAnimation("Walk");
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {

    }
}
