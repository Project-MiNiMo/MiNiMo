using UnityEngine;
using TMPro;

public class QuestInstallAdvancedFruit : QuestBase
{
    public override string ID => "InstallAdvanced_Fruit";
    protected override bool IsClear => CheckClear();

    [SerializeField] private Transform _builidngParent;
    [SerializeField] private TextMeshProUGUI _detailText;
    
    protected override void ClearQuest()
    {
        base.ClearQuest();
        
        _detailText.text = "과수 공방을 설치하세요. (1/1)";
    }
    
    private bool CheckClear()
    {
        if (_builidngParent.childCount > 0)
        {
            for (var i = 0; i < _builidngParent.childCount; i++)
            {
                if (string.Equals(_builidngParent.GetChild(i).gameObject.name, "Building_OrchardFacility(Clone)"))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
