public enum MinimoState
{
    Idle,
    Walk,
    Work
}

public class MinimoFSM : FSM<Minimo, MinimoState>
{
    public MinimoFSM(Minimo owner)
    {
        AddState(MinimoState.Idle, new MinimoIdleState(owner));
        AddState(MinimoState.Walk, new MinimoWalkState(owner));
        AddState(MinimoState.Work, new MinimoWorkState(owner));
    }
}
