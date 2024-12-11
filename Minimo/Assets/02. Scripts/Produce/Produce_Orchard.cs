using System.Linq;
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
    
    public override bool StartProduce(ProduceOption option)
    {
        if (PendingTasks.Count > 0 || CompleteTasks.Count > 0)
        {
            return false;
        }

        if (base.StartProduce(option))
        {
            PendingTasks.Add(new ProduceTask(option));
            PendingTasks.Add(new ProduceTask(option));
            PendingTasks.Add(new ProduceTask(option));
            PendingTasks.Add(new ProduceTask(option));
        }

        return true;
    }
}
