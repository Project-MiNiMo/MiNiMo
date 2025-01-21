using System;
using System.Linq;
using System.Collections.Generic;
using MinimoShared;
using Cysharp.Threading.Tasks;

using UnityEngine;

public abstract class ProduceObject : BuildingObject
{
    public ProduceData ProduceData { get; private set; }
    public List<ProduceTask> AllTasks { get; } = new(); 
    public ProduceTask ActiveTask { get; private set; }
    public virtual bool IsPrimary => false;

    private bool[] _produceSlots = new bool[5];
    private bool _isPlanting = false;
    private bool _isHarvesting = false;

    private PlantHelper _plantHelper;
    
    private ProduceManager _produceManager;
    private TimeManager _timeManager;
    private DateTime _lastUpdateTime;

    public override void Initialize(BuildingData data)
    {
        base.Initialize(data);

        ProduceData = App.GetData<TitleData>().Produce[data.ID];

        _plantHelper = new PlantHelper();
        
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
            ActiveTask.ChangeState(ActiveState.Instance);
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

    public void StartProduce(ProduceOption option)
    {
        if (!ProduceData.ProduceOptions.Contains(option)) return;
        if (_isPlanting) return;
        
        var slotIndex = Array.FindIndex(_produceSlots, slot => !slot);
        
        if (slotIndex == -1) return;
        
        var optionIndex = Array.IndexOf(ProduceData.ProduceOptions, option);
        
        _plantHelper.TryPlant(
            option,
            optionIndex,
            slotIndex,
            OnPlant
        );
    }

    protected virtual async UniTask OnPlant(ProduceTask task, int optionIndex)
    {
        _isPlanting = true;
        
        var newStartProduce = new BuildingStartProduceDTO
        {
            BuildingId = _id,
            SlotIndex = task.SlotIndex,
            RecipeId = ++optionIndex
        };
        
        await _buildingManager.StartProduce(newStartProduce);
        
        AllTasks.Add(task);
        Debug.Log($"ProduceTask Added : {task.Data.Results[0].Code}");
        SetNextActiveTask();

        _isPlanting = false;
    }

    public virtual async UniTask StartHarvest()
    {
        if (_isHarvesting) return;
        _isHarvesting = true;
        
        for (var i = AllTasks.Count - 1; i >= 0; i--)
        {
            var task = AllTasks[i];
            if (task.CurrentState is CompletedState)
            {
                var newCompleteProduce = new BuildingCompleteProduceDTO
                {
                    BuildingId = _id,
                    SlotIndex = task.SlotIndex
                };
                await _buildingManager.CompleteProduce(newCompleteProduce);
                
                task.Harvest();
                AllTasks.RemoveAt(i);
            }
        }

        SetNextActiveTask();
        
        _isHarvesting = false;
    }

    public async UniTask HarvestEarly()
    {
        if (ActiveTask == null) return;
        
        var newInstantProduce = new BuildingInstantProduceDTO
        {
            BuildingId = _id,
            SlotIndex = ActiveTask.SlotIndex
        };
        await _buildingManager.InstantProduceAsync(newInstantProduce);
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