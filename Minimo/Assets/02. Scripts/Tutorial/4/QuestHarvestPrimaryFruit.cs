public class QuestHarvestPrimaryFruit : QuestBase
{
    public override string ID => "HarvestPrimary_Fruit";
    protected override bool IsClear => CheckClear();
    
    private StorageBtn _storageBtn;
    
    public override void StartQuest()
    {
        base.StartQuest();

        var storagePanel = App.GetManager<UIManager>().GetPanel<StoragePanel>();
        _storageBtn = storagePanel.GetStorageBtn("Item_OrchardWood");
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