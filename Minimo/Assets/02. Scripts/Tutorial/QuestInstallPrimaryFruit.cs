using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInstallPrimaryFruit : QuestBase
{
    public override string ID => "InstallPrimary_Fruit";
    protected override bool IsClear => _isClear;

    private bool _isClear = false;
    
    protected override void ShowDetail()
    {
        //App.GetManager<UIManager>().OpenPanel<UIQuestDetail>(this);
    }
    
    public void Clear()
    {
        _isClear = true;
    }
}
