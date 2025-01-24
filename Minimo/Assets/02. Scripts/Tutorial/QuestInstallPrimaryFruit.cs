using UnityEngine;
using TMPro;

public class QuestInstallPrimaryFruit : QuestBase
{
    public override string ID => "InstallPrimary_Fruit";
    protected override bool IsClear => CheckClear();

    [SerializeField] private Transform _builidngParent;
    [SerializeField] private TextMeshProUGUI _detailText;
    
    protected override void ShowDetail()
    {
        //App.GetManager<UIManager>().OpenPanel<UIQuestDetail>(this);
    }
    
    protected override void ClearQuest()
    {
        base.ClearQuest();
        
        _detailText.text = "과수원을 설치하세요. (1/1)";
    }
    
    private bool CheckClear()
    {
        if (_builidngParent.childCount > 0)
        {
            for (var i = 0; i < _builidngParent.childCount; i++)
            {
                if (string.Equals(_builidngParent.GetChild(i).gameObject.name, "Building_Orchard(Clone)"))
                {
                    return true;
                }
            }
        }

        return false;
    }
}