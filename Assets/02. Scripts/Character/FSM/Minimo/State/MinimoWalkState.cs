using UnityEngine;

public class MinimoWalkState : StateBase<Minimo>
{
    private const float RADIUS = 10;

    private readonly PathFinder _path;

    public MinimoWalkState(Minimo owner) : base(owner) 
    {
        _path = owner.GetComponent<PathFinder>();
    }

    public override void Enter()
    {
        if (_path.SetTarget(GetRandomPosition(), OnEndPathFinding))
        {
            _owner.SetAnimation("Walk");
        }
        else
        {
            _owner.SetChillState();
        }
    }

    public override void Execute() { }

    public override void Exit()
    {
        _path.StopPathFinding();
    }

    private Vector3 GetRandomPosition()
    {
        var center = _owner.transform.position;

        Vector2 randomCircle = Random.insideUnitCircle * RADIUS;
        Vector3 randomPosition = new(center.x + randomCircle.x, center.y + randomCircle.y, center.z);

        return randomPosition;
    }

    private void OnEndPathFinding()
    {
        _owner.SetChillState();
    }
}
