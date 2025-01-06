using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class ProduceObject : BuildingObject
{
    public ProduceData ProduceData { get; private set; }
    public List<ProduceTask> AllTasks { get; } = new(); 
    public ProduceTask ActiveTask { get; private set; }
    public virtual bool IsPrimary => false;

    private ProduceManager _produceManager;
    private TimeManager _timeManager;
    private DateTime _lastUpdateTime; 

    public override void Initialize(BuildingData data)
    {
        base.Initialize(data);

        ProduceData = App.GetData<TitleData>().Produce[data.ID];

        _produceManager = App.GetManager<ProduceManager>();
        _timeManager = App.GetManager<TimeManager>();
        _lastUpdateTime = _timeManager.Time;
    }

    protected override void Update()
    {
        base.Update();
        
        if ((_timeManager.Time - _lastUpdateTime).TotalSeconds < 1f) return;

        _lastUpdateTime = _timeManager.Time;

        if (ActiveTask == null) return;
        
        ActiveTask.Update();
        UpdateRemainTime();
        
        if (ActiveTask is { RemainTime: <= 0 })
        {
            CompleteActiveTask();
        }
    }
    
    protected virtual void CompleteActiveTask()
    {
        ActiveTask = null;

        SetNextActiveTask();
    }
    
    private void SetNextActiveTask()
    {
        if (ActiveTask != null)
        {
            return;
        }
        
        ActiveTask = AllTasks.FirstOrDefault(task => task.CurrentState is PendingState);

        if (ActiveTask != null)
        {
            ActiveTask.ChangeState(new ActiveState());
            UpdateRemainTime();
        }
    }
    
    private void UpdateRemainTime()
    {
        if (_produceManager.CurrentProduceObject == this && ActiveTask != null)
        {
            _produceManager.SetRemainTime(ActiveTask.RemainTime);
        }
    }

    public virtual bool StartProduce(ProduceOption option)
    {
        if (!ProduceData.ProduceOptions.Contains(option))
        {
            return false;
        }

        AllTasks.Add(new ProduceTask(option));
        SetNextActiveTask();
        
        return true;
    }

    public virtual void StartHarvest()
    {
        for (var i = AllTasks.Count - 1; i >= 0; i--)
        {
            var task = AllTasks[i];
            if (task.CurrentState is CompletedState)
            {
                task.Harvest();
                AllTasks.RemoveAt(i);
            }
        }

        SetNextActiveTask();
    }

    public void HarvestEarly()
    {
        if (ActiveTask == null) return;
        
        ActiveTask.Harvest();
        CompleteActiveTask();
    }
    
    public void OrganizeTasks()
    {
        AllTasks.RemoveAll(task => task.CurrentState is EndState);
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