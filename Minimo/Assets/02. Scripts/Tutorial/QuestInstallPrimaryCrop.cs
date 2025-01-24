using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInstallPrimaryCrop : QuestBase
{
    public override string ID => "InstallPrimary_Crop";
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
