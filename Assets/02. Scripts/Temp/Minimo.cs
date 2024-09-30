using UnityEngine;

public class Minimo : MonoBehaviour
{
    public MinimoFSM FSM { get; private set; }
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        FSM = new(this);
    }

    private void Update()
    {
        FSM.Update();
    }

    public void SetAnimation(string _trigger)
    {
        Debug.Log($"Minimo State Change {_trigger}");

        animator.SetTrigger(_trigger);
    }
}
