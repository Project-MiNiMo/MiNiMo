using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestHarvestPrimaryCrop : QuestBase
{
    public override string ID => "HarvestPrimary_Crop";
    protected override bool IsClear => CheckClear();
    
    private List<StorageBtn> _storageBtns;
    
    public override void StartQuest()
    {
        base.StartQuest();

        var storagePanel = App.GetManager<UIManager>().GetPanel<StoragePanel>();
        
        _storageBtns = new List<StorageBtn>(5)
        {
            storagePanel.GetStorageBtn("Item_Wheat"),
            storagePanel.GetStorageBtn("Item_Corn"),
            storagePanel.GetStorageBtn("Item_Pumpkin"),
            storagePanel.GetStorageBtn("Item_Sugarcane"),
            storagePanel.GetStorageBtn("Item_Pepper")
        };
    }
    
    protected override void ShowDetail()
    {
        //App.GetManager<UIManager>().OpenPanel<UIQuestDetail>(this);
    }

    private bool CheckClear()
    {
        return _storageBtns.Any(x => x.CanShow);
    }
}