using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceObject : BuildingObject
{
    public ProduceData ProduceData { get; private set; }
    public int RemainTime { get; private set; }
    
    private ProduceManager _produceManager;
    private ProduceOption _currentOption;
    
    private float _lastUpdateTime; 
    
    public override void Initialize(BuildingData data, Sprite sprite)
    {
        base.Initialize(data, sprite);

        ProduceData = App.GetData<TitleData>().Produce[data.ID];
        _produceManager = App.GetManager<ProduceManager>();
    }

    //TODO : Connect with Server and remain time
    private void Update()
    {
        if (_currentOption == null || RemainTime < 0) 
        {
            return;
        }
        
        if (Time.time - _lastUpdateTime >= 1f)
        {
            _lastUpdateTime = Time.time;

            if (_produceManager.CurrentProduceObject == this)
            {
                _produceManager.SetRemainTime(--RemainTime);
            }
        }
    }
    
    protected override void OnClickWhenNotEditing()
    {
        _produceManager.ActiveProduce(this);
    }
    
    public void StartProduce(int optionNumber)
    {
        _currentOption = ProduceData.ProduceOptions[optionNumber];
        RemainTime = _currentOption.Time;
        
        _produceManager.SetRemainTime(RemainTime);
    }
}
