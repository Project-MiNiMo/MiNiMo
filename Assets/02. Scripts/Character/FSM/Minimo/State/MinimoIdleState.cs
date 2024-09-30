using UnityEngine;

public class MinimoIdleState : StateBase<Minimo>
{
    private float _stateTimer;
    private bool _isUpdating;

    private string[] _idleAnimations = { "Sit", "Stand", "Dance" };
    private string _currentAnimation;

    public MinimoIdleState(Minimo owner) : base(owner) { }

    public override void Enter()
    {
        _stateTimer = Random.Range(2f, 5f);
        _isUpdating = true;

        int randomIndex = Random.Range(0, _idleAnimations.Length);
        _currentAnimation = _idleAnimations[randomIndex];

        _owner.SetAnimation(_currentAnimation);
    }

    public override void Execute()
    {
        if (!_isUpdating) return;

        _stateTimer -= Time.deltaTime;

        if (_stateTimer <= 0)
        {
            _owner.FSM.ChangeState(MinimoState.Idle);
        }
    }

    public override void Exit()
    {
        _isUpdating = false;
    }
}
