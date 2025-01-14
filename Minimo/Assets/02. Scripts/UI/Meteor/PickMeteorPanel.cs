using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PickMeteorPanel : UIBase
{
    protected override GameObject Panel => _pickBack;
    
    [SerializeField] private GameObject _pickBack;
    
    private readonly Dictionary<MeteorType, Action> _shootingStarActions = new(4);
    private RandomItemSelector _randomItemSelector;

    private QuestPanel _questPanel;
    private GetItemPanel _getItemPanel;
    
    public override void Initialize()
    {
        _randomItemSelector = new RandomItemSelector();
        
        var uiManager = App.GetManager<UIManager>();
        _questPanel = uiManager.GetPanel<QuestPanel>();
        _getItemPanel = uiManager.GetPanel<GetItemPanel>();

        _shootingStarActions[MeteorType.Quest] = 
            () => _questPanel.OpenPanel(QuestType.Quest);
        _shootingStarActions[MeteorType.SpecialQuest] = 
            () => _questPanel.OpenPanel(QuestType.SpecialQuest);
        
        _shootingStarActions[MeteorType.Resource] = 
            () => _getItemPanel.OpenPanel(_randomItemSelector.GetRandomItems(ResourceType.Resource));
        _shootingStarActions[MeteorType.SpecialResource] = 
            () => _getItemPanel.OpenPanel(_randomItemSelector.GetRandomItems(ResourceType.SpecialResource));
    }

    public void OpenPanel(MeteorType type)
    {
        OpenPanel();
        
        StartCoroutine(Harvest(type));
    }

    private IEnumerator Harvest(MeteorType type)
    {
        yield return new WaitForSeconds(1);

        ClosePanel();
        
        GetPickAction(type).Invoke();
    }
    
    private Action GetPickAction(MeteorType type)
    {
        return _shootingStarActions[type];
    }
}
