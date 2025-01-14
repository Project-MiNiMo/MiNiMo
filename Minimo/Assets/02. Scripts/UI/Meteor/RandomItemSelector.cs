using System.Collections.Generic;

using UnityEngine;

public class RandomItemSelector
{
    private readonly ItemSO _itemSO = App.GetData<TitleData>().ItemSO;

    public List<Item> GetRandomItems(ResourceType type, int count = 1)
    {
        var items = new List<Item>();

        for (var i = 0; i < count; i++) 
        {
            var item = GetRandomItem(type);
            items.Add(item);
        }

        return items;
    }
    
    private Item GetRandomItem(ResourceType type)
    {
        var randomIndex = Random.Range(0, _itemSO.items.Count);
        return _itemSO.items[randomIndex];
    }
}
