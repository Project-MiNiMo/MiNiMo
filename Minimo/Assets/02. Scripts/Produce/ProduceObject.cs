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
    
    private ProduceManager _produceManager;
    private float _lastUpdateTime; 
    
    public override void Initialize(BuildingData data)
    {
        base.Initialize(data);

        ProduceData = App.GetData<TitleData>().Produce[data.ID];
        _produceManager = App.GetManager<ProduceManager>();
        
        CurrentState = ProduceState.Idle;
    }

    //TODO : Connect with Server and remain time
    protected override void Update()
    {
        base.Update();
        
        if (CurrentState != ProduceState.Produce) 
        {
            return;
        }
        
        if (Time.time - _lastUpdateTime >= 1f)
        {
            _lastUpdateTime = Time.time;
            _remainTime--;
            
            if (_produceManager.CurrentProduceObject == this)
            {
                _produceManager.SetRemainTime(_remainTime);
            }

            if (_remainTime == 0)
            {
                SetupHarvest();
            }
            
            CheckInUpdate();
        }
    }
    
    protected abstract void CheckInUpdate();
    
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
        
        Debug.Log("Start Produce : " + option.Results[0].Code);

        SetupProduce();
    }
    
    protected virtual void SetupProduce()
    {
        CurrentState = ProduceState.Produce;
    }

    public void StartHarvest()
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
