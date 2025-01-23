public class MinimoDragState : StateBase<Minimo>
{
    public MinimoDragState(Minimo owner) : base(owner) { }

    public override void Enter()
    {
        _owner.SetAnimation("IsDrag", true);
    }

    public override void Execute() { }

    public override void Exit()
    {
        _owner.SetAnimation("IsDrag", false);
    }
}
