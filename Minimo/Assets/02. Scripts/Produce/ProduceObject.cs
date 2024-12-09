using System.Linq;

using UnityEngine;

public class ProduceObject : BuildingObject
{
    public ProduceData ProduceData { get; private set; }
    public ProduceOption CurrentOption { get; private set; }
    public bool IsProducing => _remainTime > 0;
    
    protected int _remainTime;
    
    private ProduceManager _produceManager;
    private float _lastUpdateTime; 
    
    public override void Initialize(BuildingData data, Sprite sprite)
    {
        base.Initialize(data, sprite);

        ProduceData = App.GetData<TitleData>().Produce[data.ID];
        _produceManager = App.GetManager<ProduceManager>();
    }

    //TODO : Connect with Server and remain time
    protected virtual void Update()
    {
        if (CurrentOption == null || _remainTime < 0) 
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
        }
    }
    
    protected override void OnClickWhenNotEditing()
    {
        _produceManager.ActiveProduce(this);
        
        _produceManager.SetRemainTime(_remainTime);
    }
    
    public virtual void StartProduce(ProduceOption option)
    {
        if (IsProducing) 
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
    }
}
