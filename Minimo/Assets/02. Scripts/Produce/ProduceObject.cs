using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public abstract class ProduceObject : BuildingObject
{
    public ProduceData ProduceData { get; private set; }
    public List<ProduceTask> AllTasks { get; } = new(); 
    public int ActiveTaskIndex { get; private set; } = -1; 

    private ProduceManager _produceManager;
    private TimeManager _timeManager;
    private DateTime _lastUpdateTime; 

    public override void Initialize(BuildingData data)
    {
        base.Initialize(data);

        ProduceData = App.GetData<TitleData>().Produce[data.ID];

        _produceManager = App.GetManager<ProduceManager>();
        _timeManager = App.GetManager<TimeManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (ActiveTaskIndex >= 0) 
        {
            UpdateProduceTasks();
        }
    }

    private void UpdateProduceTasks()
    {
        if ((_timeManager.Time - _lastUpdateTime).TotalSeconds < 1f) return;

        _lastUpdateTime = _timeManager.Time;

        var activeTask = AllTasks[ActiveTaskIndex];
        activeTask.UpdateHarvestTime();

        SetRemainTime(activeTask);

        if (activeTask.RemainTime <= 0) 
        {
            SetCompleteTask();
            SetActiveTask();
        }
    }

    private void SetRemainTime(ProduceTask task)
    {
        if (_produceManager.CurrentProduceObject == this)
        {
            _produceManager.SetRemainTime(task.RemainTime);
        }
    }

    protected virtual void SetCompleteTask()
    {
        if (ActiveTaskIndex >= 0) 
        {
            ActiveTaskIndex = -1;
        }
    }

    protected void SetActiveTask()
    {
        if (ActiveTaskIndex >= 0) return;

        var nextTaskIndex = AllTasks.FindIndex(t => t.RemainTime > 0);
        if (nextTaskIndex >= 0)
        {
            ActiveTaskIndex = nextTaskIndex;
            SetRemainTime(AllTasks[ActiveTaskIndex]);
        }
    }

    public virtual bool StartProduce(ProduceOption option)
    {
        if (!ProduceData.ProduceOptions.Contains(option))
        {
            return false;
        }

        AllTasks.Add(new ProduceTask(option));
        SetActiveTask();
        Debug.Log("Start Produce : " + option.Results[0].Code);

        return true;
    }

    public virtual void StartHarvest()
    {
        foreach (var task in AllTasks.Where(t => t.RemainTime <= 0))
        {
            foreach (var result in task.Data.Results)
            {
                var item = App.GetData<TitleData>().ItemSO.GetItem(result.Code);
                item.Count += result.Amount;
            }
        }

        AllTasks.RemoveAll(t => t.RemainTime <= 0);
    }

    public void HarvestEarly()
    {
        if (ActiveTaskIndex < 0) return;

        var activeTask = AllTasks[ActiveTaskIndex];
        activeTask.HarvestEarly();
        SetRemainTime(activeTask);

        SetCompleteTask();
        SetActiveTask();
    }

    protected override void OnClickWhenNotEditing()
    {
        _produceManager.ActiveProduce(this);

        if (ActiveTaskIndex >= 0) 
        {
            _produceManager.SetRemainTime(AllTasks[ActiveTaskIndex].RemainTime);
        }
    }
}