using System.Collections.Generic;

using UnityEngine;

public class MinimoWalkState : StateBase<Minimo>
{
    public MinimoWalkState(Minimo owner) : base(owner) { }

    private const float SPEED = 0.3f;

    private bool _isUpdating = false;

    private List<Vector3Int> _path;
    private Vector3 _targetPosition;

    private int _currIndex;

    public override void Enter()
    {
        _path = App.Instance.GetManager<PathManager>().GetRandomPath(_owner.transform.position);

        if (_path != null && _path.Count > 0)
        {
            _owner.SetAnimation("Walk");

            _currIndex = 0;
            _targetPosition = App.Instance.GetManager<PathManager>().GetTileWorldPosition(_path[_currIndex]);

            _isUpdating = true;
        }
        else
        {
            _owner.SetChillState();
        }
    }

    public override void Execute()
    {
        if (!_isUpdating)
        { 
            return;
        }

        if (_currIndex < _path.Count)
        {
            if ((_targetPosition - _owner.transform.position).sqrMagnitude > 0.01f)
            {
                _owner.transform.position = Vector3.MoveTowards(_owner.transform.position, _targetPosition, SPEED * Time.deltaTime);
            }
            else
            {
                if (++_currIndex < _path.Count)
                {
                    _targetPosition = App.Instance.GetManager<PathManager>().GetTileWorldPosition(_path[_currIndex]);
                }
            }
        }
        else
        {
            _isUpdating = false;
            _owner.SetChillState();
        }
    }

    public override void Exit()
    {
        _isUpdating = false;
        _path = null;
    }
}
