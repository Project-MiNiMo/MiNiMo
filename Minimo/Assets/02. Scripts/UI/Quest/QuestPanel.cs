using UnityEngine;
using UnityEngine.UI;

public enum QuestType
{
    Quest,
    SpecialQuest
}

public class QuestPanel : UIBase
{
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _denyBtn;

    public override void Initialize() 
    {
        _confirmBtn.onClick.AddListener(OnClickConfirm);
        _denyBtn.onClick.AddListener(OnClickDeny);
    }
    
    public void OpenPanel(QuestType type)
    {
        base.OpenPanel();
    }

    private void OnClickConfirm()
    {
        ClosePanel();
    }

    private void OnClickDeny()
    {
        ClosePanel();
    }
}
