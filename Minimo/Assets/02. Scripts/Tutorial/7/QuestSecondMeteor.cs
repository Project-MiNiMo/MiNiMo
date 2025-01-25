using UnityEngine;
using TMPro;

public class QuestSecondMeteor : QuestBase
{
    public override string ID => "SecondMeteor";
    protected override bool IsClear => CheckClear();
    
    private StorageBtn _storageBtn;
    [SerializeField] private TextMeshProUGUI _detailText;
    
    public override void StartQuest()
    {
        base.StartQuest();

        var storagePanel = App.GetManager<UIManager>().GetPanel<StoragePanel>();
        _storageBtn = storagePanel.GetStorageBtn("Item_WheatFlour");
    }
    
    protected override void ClearQuest()
    {
        base.ClearQuest();
        
        _detailText.text = "<color=red>유성 속에 담긴 소원을 들어주세요.</color>\n밀가루를 생산하세요. (1/1)";
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
