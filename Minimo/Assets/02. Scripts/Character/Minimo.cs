using UnityEngine;

public class Minimo : MonoBehaviour
{
    public MinimoFSM FSM { get; private set; }

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        FSM = new(this);
        SetChillState();
    }

    private void Update()
    {
        FSM.Update();
    }

    public void SetAnimation(string _trigger)
    {
        Debug.Log($"Minimo State Change {_trigger}");

        _animator.SetTrigger(_trigger);
    }

    public void SetChillState()
    {
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            FSM.ChangeState(MinimoState.Idle);
        }
        else
        {
            FSM.ChangeState(MinimoState.Walk);
        }
    }
}
