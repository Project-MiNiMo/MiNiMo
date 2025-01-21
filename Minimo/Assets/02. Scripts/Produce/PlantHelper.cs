using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class PlantHelper 
{
    private readonly ItemSO _itemSO = App.GetData<TitleData>().ItemSO;
    private readonly UseCashPanel _useCashPanel = App.GetManager<UIManager>().GetPanel<UseCashPanel>();

    public void TryPlant(
        ProduceOption option,
        int optionIndex,
        Func<ProduceTask, int, UniTask> onTaskCreated)
    {
        var lackItems = GetLackItems(option.Materials);

        if (lackItems.Count > 0)
        {
            _useCashPanel.OpenPanel(lackItems, async () =>
            {
                await CreateTaskAsync(option, optionIndex, onTaskCreated);
            });

            return;
        }

        CreateTaskAsync(option, optionIndex, onTaskCreated).Forget();
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
    
    private async UniTask CreateTaskAsync(
        ProduceOption option, 
        int optionIndex, 
        Func<ProduceTask, int, UniTask> onTaskCreated)
    {
        ConsumeMaterials(option.Materials);

        var newTask = new ProduceTask(option);
        await onTaskCreated(newTask, optionIndex);
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
