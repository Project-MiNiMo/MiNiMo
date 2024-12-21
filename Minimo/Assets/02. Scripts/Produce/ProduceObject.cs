using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public abstract class ProduceObject : BuildingObject
{
    public ProduceData ProduceData { get; private set; }
    public ProduceTask ActiveTask { get; private set; }
    public List<ProduceTask> CompleteTasks { get; } = new();
    public List<ProduceTask> PendingTasks { get; } = new();
    
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

        if (ActiveTask != null) 
        {
            UpdateProduceTasks();
        }
    }

    private void UpdateProduceTasks()
    {
        if ((_timeManager.Time - _lastUpdateTime).TotalSeconds < 1f) return;
        
        _lastUpdateTime = _timeManager.Time;
        
        ActiveTask.UpdateHarvestTime();

        SetRemainTime();
        
        if (ActiveTask.RemainTime == 0) 
        {
            SetCompleteTask();
            SetActiveTask();
        }
    }

    private void SetRemainTime()
    {
        if (_produceManager.CurrentProduceObject == this)
        {
            _produceManager.SetRemainTime(ActiveTask.RemainTime);
        }
    }

    protected virtual void SetCompleteTask()
    {
        if (ActiveTask != null) 
        {
            CompleteTasks.Add(ActiveTask);
            ActiveTask = null;
        }
    }

    protected void SetActiveTask()
    {
        if (ActiveTask != null) return;
        
        if (PendingTasks.Count > 0)
        {
            ActiveTask = PendingTasks[0];
            PendingTasks.RemoveAt(0);

            SetRemainTime();
        }
    }
    
    public virtual bool StartProduce(ProduceOption option)
    {
        if (!ProduceData.ProduceOptions.Contains(option))
        {
            return false;
        }
 
        PendingTasks.Add(new ProduceTask(option));
        SetActiveTask();
        Debug.Log("Start Produce : " + option.Results[0].Code);

        return true;
    }
  
    public virtual void StartHarvest()
    {
        foreach (var task in CompleteTasks)
        {
            foreach (var result in task.Data.Results)
            {
                var item = App.GetData<TitleData>().ItemSO.GetItem(result.Code);
                item.Count += result.Amount;
            }
        }
        
        CompleteTasks.Clear();
    }

    public void HarvestEarly()
    {
        if (ActiveTask == null) return;
        
        ActiveTask.HarvestEarly();
        SetRemainTime();
        
        SetCompleteTask();
        SetActiveTask();
    }
    
    protected override void OnClickWhenNotEditing()
    {
        _produceManager.ActiveProduce(this);

        if (ActiveTask != null) 
        {
            _produceManager.SetRemainTime(ActiveTask.RemainTime);
        }
    }
}
