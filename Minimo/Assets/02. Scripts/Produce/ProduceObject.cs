using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public abstract class ProduceObject : BuildingObject
{
    public ProduceData ProduceData { get; private set; }
    public ProduceTask ActiveTask { get; private set; }
    
    private readonly List<ProduceTask> _pendingTasks = new();
    private readonly List<ProduceTask> _completeTasks = new();
    
    protected ProduceManager _produceManager;
    protected TimeManager _timeManager;
    protected DateTime _lastUpdateTime; 
    
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
        
        if (ActiveTask.RemainTime == 0) 
        {
            SetCompleteTask();
            SetActiveTask();
        }
    }

    private void SetCompleteTask()
    {
        if (ActiveTask != null) 
        {
            _completeTasks.Add(ActiveTask);
            ActiveTask = null;
        }
    }

    private void SetActiveTask()
    {
        if (ActiveTask != null) return;
        
        if (_pendingTasks.Count > 0)
        {
            ActiveTask = _pendingTasks[0];
            _pendingTasks.RemoveAt(0);
        }
    }
    
    public void StartProduce(ProduceOption option)
    {
        if (!ProduceData.ProduceOptions.Contains(option))
        {
            return;
        }
 
        _pendingTasks.Add(new ProduceTask(option));
        SetActiveTask();
        Debug.Log("Start Produce : " + option.Results[0].Code);
    }
  
    public void StartHarvest()
    {
        foreach (var task in _completeTasks)
        {
            foreach (var result in task.Data.Results)
            {
                var item = App.GetData<TitleData>().ItemSO.GetItem(result.Code);
                item.Count += result.Amount;
            }
        }
        
        _completeTasks.Clear();
    }

    public void HarvestEarly()
    {
        if (ActiveTask == null) return;
        
        ActiveTask.HarvestEarly();
        SetCompleteTask();
        SetActiveTask();
    }
    
    protected override void OnClickWhenNotEditing()
    {
        _produceManager.ActiveProduce(this);
    }
}
