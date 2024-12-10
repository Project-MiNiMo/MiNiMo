using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Produce_Orchard :ProduceObject
{
    private enum FruitType
    {
        Apple,
        Blueberry,
        Pineapple,
    }

    [SerializeField] private SpriteRenderer _fruitSpriteRenderer;
    
    private List<Sprite[]> _fruitSprites;
   
    private Sprite[] _currentFruitSprites;
    private int _currentSpriteIndex;

    private int _harvestIndex = 5;
    private int _canHarvestCount = 0;
    
    public override void Initialize(BuildingData data)
    {
        base.Initialize(data);

        _fruitSprites = new List<Sprite[]>(ProduceData.ProduceOptions.Length)
        {
            Resources.LoadAll<Sprite>("Produce/Orchard/Apple"),
            Resources.LoadAll<Sprite>("Produce/Orchard/Blueberry"),
            Resources.LoadAll<Sprite>("Produce/Orchard/Pineapple"),
        };
    }
    
    protected override bool IsNotProducing => _harvestIndex >= 5;

    protected override void CheckRemainTime()
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
                _harvestIndex++;
                _canHarvestCount++;
                _remainTime = CurrentOption.Time;
            }
            
            SetProduceSprite();
        }
    }

    protected override void SetProduceSprite()
    {
        if (_currentFruitSprites == null) 
        {
            return;
        }

        if (_canHarvestCount > 0) 
        {
            _fruitSpriteRenderer.sprite = _currentFruitSprites[2];
            return;
        }
        
        var remainPercent = (float)_remainTime / CurrentOption.Time;

        var newSpriteIndex = remainPercent switch
        {
            >= 0.5f => 0,
            >= 0.01f => 1,
            _ => 2
        };

        if (newSpriteIndex != _currentSpriteIndex)
        {
            _currentSpriteIndex = newSpriteIndex;
            _fruitSpriteRenderer.sprite = _currentFruitSprites[_currentSpriteIndex];
        }
    }
    
    protected override void SetupIdle()
    {
        base.SetupIdle();

        _fruitSpriteRenderer.sprite = null;
    }
    
    protected override void SetupProduce()
    {
        base.SetupProduce();
        
        _currentSpriteIndex = 0;

        var fruitCode = CurrentOption.Results[0].Code;
        var currentFruitType = fruitCode switch
        {
            "Item_Apple" => FruitType.Apple,
            "Item_Blueberry" => FruitType.Blueberry,
            "Item_Pineapple" => FruitType.Pineapple,
            _ => FruitType.Apple
        };
        
        _currentFruitSprites = _fruitSprites[(int)currentFruitType];
        _fruitSpriteRenderer.sprite = _currentFruitSprites[_currentSpriteIndex];

        _harvestIndex = 0;
    }

    public override void StartHarvest()
    {
        if (CurrentState != ProduceState.Harvest) 
        {
            return;
        }
        
        if (_harvestIndex >= 5)
        {
            SetupIdle();
        }
        else if (_canHarvestCount > 0)
        {
            _canHarvestCount = 0;
            SetProduceSprite();
            base.SetupProduce();
        }
    }
    
    protected override void SetupHarvest()
    {
        base.SetupHarvest();
        
        //var result = CurrentOption.Results[0];
        //var itemCode = result.Code;
        //var amount = result.Amount;
        
        //App.GetManager<InventoryManager>().AddItem(itemCode, amount);
        
        _currentSpriteIndex = 2;
        _fruitSpriteRenderer.sprite = _currentFruitSprites[_currentSpriteIndex];
    }
}
