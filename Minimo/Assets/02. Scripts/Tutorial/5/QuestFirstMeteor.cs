using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFirstMeteor : QuestBase
{
    public override string ID => "FirstMeteor";
    protected override bool IsClear => CheckClear();
    [SerializeField] private Meteor _meteor;
    
    public override void StartQuest()
    {
        base.StartQuest();

        _meteor.enabled = true;
    }
    
    protected override void ShowDetail()
    {
        //App.GetManager<UIManager>().OpenPanel<UIQuestDetail>(this);
    }

    private bool CheckClear()
    {
        return true;
    }
}
