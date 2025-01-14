using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PickMeteorPanel : UIBase
{
    protected override GameObject Panel => _pickBack;
    
    [SerializeField] private GameObject _pickBack;
    
    private readonly Dictionary<MeteorType, Action> _shootingStarActions = new(4);

    private QuestPanel _questPanel;
    private ResourcePanel _resourcePanel;
    
    public override void Initialize()
    {
        var uiManager = App.GetManager<UIManager>();
        _questPanel = uiManager.GetPanel<QuestPanel>();
        _resourcePanel = uiManager.GetPanel<ResourcePanel>();

        _shootingStarActions[MeteorType.Quest] = () => _questPanel.OpenPanel(QuestType.Quest);
        _shootingStarActions[MeteorType.SpecialQuest] = () => _questPanel.OpenPanel(QuestType.SpecialQuest);
        _shootingStarActions[MeteorType.Resource] = _resourcePanel.OpenPanel;
        _shootingStarActions[MeteorType.SpecialResource] = _resourcePanel.OpenPanel;
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
