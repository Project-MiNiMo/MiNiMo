using System;
using System.Collections.Generic;

using UnityEngine;

public class PlantHelper 
{
    private readonly ItemSO _itemSO;
    private readonly DiamondPanel _diamondPanel;

    public PlantHelper()
    {
        _itemSO = App.GetData<TitleData>().ItemSO;
        _diamondPanel = App.GetManager<UIManager>().GetPanel<DiamondPanel>();
    }
    
    public void TryPlant(
        ProduceOption option,
        Action<ProduceTask> onTaskCreated)
    {
        var lackItems = GetLackItems(option.Materials);

        if (lackItems.Count > 0)
        {
            _diamondPanel.OpenPanel(lackItems, () =>
            {
                CreateTask(option, onTaskCreated);
            });

            return;
        }

        CreateTask(option, onTaskCreated);
    }
    
    private List<(Item, int)> GetLackItems(ProduceMaterial[] materials)
    {
        var lackItems = new List<(Item, int)>();

        foreach (var material in materials)
        {
            var item = _itemSO.GetItem(material.Code);
            if (item.Count < material.Amount)
            {
                lackItems.Add((item, material.Amount - item.Count));
            }
        }

        return lackItems;
    }
    
    private void CreateTask(ProduceOption option, Action<ProduceTask> onTaskCreated)
    {
        ConsumeMaterials(option.Materials);

        var newTask = new ProduceTask(option);
        onTaskCreated?.Invoke(newTask);
    }

    private void ConsumeMaterials(ProduceMaterial[] materials)
    {
        foreach (var material in materials)
        {
            var item = _itemSO.GetItem(material.Code);
            item.Count -= material.Amount;
        }
    }
}
