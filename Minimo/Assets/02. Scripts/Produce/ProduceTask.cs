using UnityEngine;

public class ProduceTask
{
    public ProduceOption Data { get; }
    public int RemainTime { get; private set; }
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

    public void ReduceRemainTime(int amount)
    {
        RemainTime = Mathf.Max(0, RemainTime - amount);
    }
}

public interface ITaskState
{
    void OnUpdate(ProduceTask task);
    void OnHarvest(ProduceTask task);
}

public class PendingState : ITaskState
{
    public void OnUpdate(ProduceTask task) { }

    public void OnHarvest(ProduceTask task) { }
}

public class ActiveState : ITaskState
{
    public void OnUpdate(ProduceTask task)
    {
        task.ReduceRemainTime(1);
        if (task.RemainTime <= 0)
        {
            task.ChangeState(new CompletedState());
        }
    }

    public void OnHarvest(ProduceTask task)
    {
        task.ReduceRemainTime(task.RemainTime);
        task.ChangeState(new CompletedState());
    }
}

public class CompletedState : ITaskState
{
    public void OnUpdate(ProduceTask task) { }

    public void OnHarvest(ProduceTask task)
    {
        Debug.Log($"Harvested: {task.Data.Results}");

        var titleData = App.GetData<TitleData>();
        
        foreach (var result in task.Data.Results)
        {
            var item = titleData.ItemSO.GetItem(result.Code);
            item.Count += result.Amount;
        }
        
        task.ChangeState(new EndState());
    }
}

public class EndState : ITaskState
{
    public void OnUpdate(ProduceTask task) { }

    public void OnHarvest(ProduceTask task) { }
}