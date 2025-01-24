using System;
using System.Collections;
using MinimoShared;

using UnityEngine;

public class PickMeteorPanel : UIBase
{
    protected override GameObject Panel => _pickBack;
    
    [SerializeField] private GameObject _pickBack;

    private QuestPanel _questPanel;
    private GetItemPanel _getItemPanel;
    
    public override void Initialize()
    {
        var uiManager = App.GetManager<UIManager>();
        _questPanel = uiManager.GetPanel<QuestPanel>();
        _getItemPanel = uiManager.GetPanel<GetItemPanel>();
    }

    public void OpenPanel(MeteorResultDTO result)
    {
        OpenPanel();
        
        StartCoroutine(Harvest(result));
    }
    
    public void OpenPanel(int index)
    {
        OpenPanel();
        
        StartCoroutine(Harvest(index));
    }

    private IEnumerator Harvest(MeteorResultDTO result)
    {
        yield return new WaitForSeconds(1);

        ClosePanel();
        
        GetPickAction(result).Invoke();
    }
    
    private IEnumerator Harvest(int index)
    {
        yield return new WaitForSeconds(2);

        ClosePanel();

        if (index == 0) 
        {
            _getItemPanel.OpenPanel(1);
        }
        else
        {
            App.GetManager<QuestManager>().StartQuest("SecondMeteor");
        }
    }
    
    private Action GetPickAction(MeteorResultDTO result)
    {
        if (result.ResultItem != null)
        {
            return () => _getItemPanel.OpenPanel(result.ResultItem.ItemType, result.ResultItem.Count);
        }
        else if (result.ResultQuest != null) 
        {
            return () => { };//_questPanel.OpenPanel(result.ResultQuest.Id);
        }
        else
        {
            return () => { };
        }
    }
}
