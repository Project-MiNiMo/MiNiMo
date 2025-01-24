using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSecondMeteor : QuestBase
{
    public override string ID => "SecondMeteor";
    protected override bool IsClear => CheckClear();
    
    private StorageBtn _storageBtn;
    
    public override void StartQuest()
    {
        base.StartQuest();

        var storagePanel = App.GetManager<UIManager>().GetPanel<StoragePanel>();
        _storageBtn = storagePanel.GetStorageBtn("Item_WheatFlour");
    }

    private bool CheckClear()
    {
        if (_storageBtn == null) 
        {
            return false;
        }
        
        return _storageBtn.CanShow;
    }
}
