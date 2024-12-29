using UnityEngine;

public class ProduceTask
{
    public ProduceOption Data { get; private set; }
    public int RemainTime;
    public ITaskState CurrentState { get; private set; }

    public ProduceTask(ProduceOption produceOption)
    {
        Data = produceOption;
        RemainTime = produceOption.Time;

        CurrentState = new PendingState();
    }

    public void Update()
    {
        CurrentState.OnUpdate(this);
    }

    public void Harvest()
    {
        CurrentState.OnHarvest(this);
    }

    public void ChangeState(ITaskState newState)
    {
        CurrentState = newState;
    }
}

public interface ITaskState
{
    void OnUpdate(ProduceTask task);
    void OnHarvest(ProduceTask task);
}

public class PendingState : ITaskState
{
    public void OnUpdate(ProduceTask task)
    {

    }

    public void OnHarvest(ProduceTask task)
    {

    }
}

public class ActiveState : ITaskState
{
    public void OnUpdate(ProduceTask task)
    {
        task.RemainTime = Mathf.Max(0, task.RemainTime - 1);
        if (task.RemainTime <= 0)
        {
            task.ChangeState(new CompletedState());
        }
    }

    public void OnHarvest(ProduceTask task)
    {
        task.RemainTime = 0;
        task.ChangeState(new CompletedState());
    }
}

public class CompletedState : ITaskState
{
    public void OnUpdate(ProduceTask task)
    {

    }

    public void OnHarvest(ProduceTask task)
    {
        Debug.Log($"Harvested: {task.Data.Results}");
    }
}