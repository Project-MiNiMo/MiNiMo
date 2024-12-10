using System;
using System.Linq;

using UnityEngine;

public enum ProduceState
{
    Idle,
    Produce,
    Harvest
}

public abstract class ProduceObject : BuildingObject
{
    public ProduceData ProduceData { get; private set; }
    public ProduceOption CurrentOption { get; private set; }
    public ProduceState CurrentState { get; private set; }
    
    protected int _remainTime;
    
    protected ProduceManager _produceManager;
    protected TimeManager _timeManager;
    protected DateTime _lastUpdateTime; 
    
    public override void Initialize(BuildingData data)
    {
        base.Initialize(data);

        CurrentState = ProduceState.Idle;
        
        ProduceData = App.GetData<TitleData>().Produce[data.ID];
        
        _produceManager = App.GetManager<ProduceManager>();
        _timeManager = App.GetManager<TimeManager>();
    }

    //TODO : Connect with Server and remain time
    protected override void Update()
    {
        base.Update();

        if (IsNotProducing) 
        {
            return;
        }
        
        CheckRemainTime();
    }
    
    protected virtual bool IsNotProducing => CurrentState != ProduceState.Produce;

    protected virtual void CheckRemainTime()
    {
        if ((_timeManager.Time - _lastUpdateTime).TotalSeconds >= 1f)
        {
            _lastUpdateTime = _timeManager.Time;
            _remainTime--;
            
            if (_produceManager.CurrentProduceObject == this)
            {
                _produceManager.SetRemainTime(_remainTime);
            }

            if (_remainTime == 0)
            {
                SetupHarvest();
            }
            
            SetProduceSprite();
        }
    }
    
    protected abstract void SetProduceSprite();
    
    protected override void OnClickWhenNotEditing()
    {
        _produceManager.ActiveProduce(this);
        
        _produceManager.SetRemainTime(_remainTime);
    }
    
    protected virtual void SetupIdle()
    {
        CurrentState = ProduceState.Idle;
    }
    
    public void StartProduce(ProduceOption option)
    {
        if (CurrentState != ProduceState.Idle) 
        {
            return;
        }
        
        if (!ProduceData.ProduceOptions.Contains(option))
        {
            return;
        }
        
        CurrentOption = option;
        _remainTime = option.Time;
        _lastUpdateTime = _timeManager.Time;
        
        Debug.Log("Start Produce : " + option.Results[0].Code);

        SetupProduce();
    }
    
    protected virtual void SetupProduce()
    {
        CurrentState = ProduceState.Produce;
    }

    public virtual void StartHarvest()
    {
        if (CurrentState != ProduceState.Harvest) 
        {
            return;
        }

        SetupIdle();
    }
    
    protected virtual void SetupHarvest()
    {
        CurrentState = ProduceState.Harvest;
    }

    public void HarvestEarly()
    {
        _remainTime = 0;
        SetupHarvest();
    }
}
