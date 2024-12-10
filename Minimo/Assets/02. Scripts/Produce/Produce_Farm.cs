using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class Produce_Farm : ProduceObject
{
    private enum CropType
    {
        Wheat,
        Corn,
        Pumpkin,
        Sugarcane,
        Pepper
    }

    [SerializeField] private SpriteRenderer _cropSpriteRenderer;
    
    private List<Sprite[]> _cropSprites;
   
    private Sprite[] _currentCropSprites;
    private int _currentSpriteIndex;
    
    public override void Initialize(BuildingData data)
    {
        base.Initialize(data);

        _cropSprites = new List<Sprite[]>(ProduceData.ProduceOptions.Length)
        {
            Resources.LoadAll<Sprite>("Produce/Farm/Wheat"),
            Resources.LoadAll<Sprite>("Produce/Farm/Corn"),
            Resources.LoadAll<Sprite>("Produce/Farm/Pumpkin"),
            Resources.LoadAll<Sprite>("Produce/Farm/Sugarcane"),
            Resources.LoadAll<Sprite>("Produce/Farm/Pepper")
        };
    }
    
    protected override void SetProduceSprite()
    {
        if (_currentCropSprites == null) 
        {
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
            _cropSpriteRenderer.sprite = _currentCropSprites[_currentSpriteIndex];
        }
    }
    
    protected override void SetupIdle()
    {
        base.SetupIdle();

        _cropSpriteRenderer.sprite = null;
    }
    
    protected override void SetupProduce()
    {
        base.SetupProduce();
        
        _currentSpriteIndex = 0;

        var cropCode = CurrentOption.Results[0].Code;
        var currentCropType = cropCode switch
        {
            "Item_Wheat" => CropType.Wheat,
            "Item_Corn" => CropType.Corn,
            "Item_Pumpkin" => CropType.Pumpkin,
            "Item_Sugarcane" => CropType.Sugarcane,
            "Item_Pepper" => CropType.Pepper,
            _ => CropType.Wheat
        };
        
        _currentCropSprites = _cropSprites[(int)currentCropType];
        _cropSpriteRenderer.sprite = _currentCropSprites[_currentSpriteIndex];
    }
    
    protected override void SetupHarvest()
    {
        base.SetupHarvest();
        
        //var result = CurrentOption.Results[0];
        //var itemCode = result.Code;
        //var amount = result.Amount;
        
        //App.GetManager<InventoryManager>().AddItem(itemCode, amount);
        
        _currentSpriteIndex = 2;
        _cropSpriteRenderer.sprite = _currentCropSprites[_currentSpriteIndex];
    }
}
