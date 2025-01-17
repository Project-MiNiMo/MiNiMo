using System.Collections.Generic;

using UnityEngine;

public class Produce_Farm : ProducePrimary
{
    private enum CropType
    {
        Wheat,
        Corn,
        Pumpkin,
        Sugarcane,
        Pepper
    }

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
    
    protected override int GetCropType(string cropCode) => cropCode switch
    {
        "Item_Wheat" => (int)CropType.Wheat,
        "Item_Corn" => (int)CropType.Corn,
        "Item_Pumpkin" => (int)CropType.Pumpkin,
        "Item_Sugarcane" => (int)CropType.Sugarcane,
        "Item_Pepper" => (int)CropType.Pepper,
        _ => (int)CropType.Wheat
    };
}
