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
    
    public override void Initialize(BuildingData data, Sprite sprite)
    {
        base.Initialize(data, sprite);

        _cropSprites = new List<Sprite[]>(ProduceData.ProduceOptions.Length)
        {
            Resources.LoadAll<Sprite>("Produce/Farm/Wheat"),
            Resources.LoadAll<Sprite>("Produce/Farm/Corn"),
            Resources.LoadAll<Sprite>("Produce/Farm/Pumpkin"),
            Resources.LoadAll<Sprite>("Produce/Farm/Sugarcane"),
            Resources.LoadAll<Sprite>("Produce/Farm/Pepper")
        };
    }

    protected override void Update()
    {
        base.Update();
        
        if (_currentCropSprites == null || _remainTime < 0) 
        {
            return;
        }
        
        var remainPercent = (float)_remainTime / CurrentOption.Time;
        int newSpriteIndex;
        
        if (remainPercent >= 0.5f)
        {
            newSpriteIndex = 0;
        }
        else if (remainPercent >= 0.01f)
        {
            newSpriteIndex = 1;
        }
        else
        {
            newSpriteIndex = 2;
        }

        if (newSpriteIndex != _currentSpriteIndex)
        {
            _currentSpriteIndex = newSpriteIndex;
            _cropSpriteRenderer.sprite = _currentCropSprites[_currentSpriteIndex];
        }
    }
    
    public override void StartProduce(ProduceOption option)
    {
        base.StartProduce(option);
        
        if (_currentCropSprites != null) 
        {
            return;
        }
     
        _currentSpriteIndex = -1;

        var cropCode = option.Results[0].Code;
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
    }
}
