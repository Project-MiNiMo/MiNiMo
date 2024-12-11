using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Produce_Orchard :ProducePrimary
{
    private enum FruitType
    {
        Apple,
        Blueberry,
        Pineapple,
    }

    public override void Initialize(BuildingData data)
    {
        base.Initialize(data);

        _cropSprites = new List<Sprite[]>(ProduceData.ProduceOptions.Length)
        {
            Resources.LoadAll<Sprite>("Produce/Orchard/Apple"),
            Resources.LoadAll<Sprite>("Produce/Orchard/Blueberry"),
            Resources.LoadAll<Sprite>("Produce/Orchard/Pineapple"),
        };
    }

    protected override int GetCropType(string cropCode) => cropCode switch
    {
        "Item_Apple" => (int)FruitType.Apple,
        "Item_Blueberry" => (int)FruitType.Blueberry,
        "Item_Pineapple" => (int)FruitType.Pineapple,
        _ => (int)FruitType.Apple
    };
    
    public override void StartHarvest()
    {
        if (_pendingTasks.Count > 0 || CompleteTasks.Count > 0)
        {
            return;
        }
        
        base.StartHarvest();
    }
}
