using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PickShootingStarPanel : UIBase
{
    [SerializeField] private GameObject _pickBack;
    
    private readonly Dictionary<ShootingStarType, Action> _shootingStarActions = new(4);

    private QuestPanel _questPanel;
    private ResourcePanel _resourcePanel;
    
    public override void Initialize()
    {
        var uiManager = App.GetManager<UIManager>();
        _questPanel = uiManager.GetPanel<QuestPanel>();
        _resourcePanel = uiManager.GetPanel<ResourcePanel>();
        
        _shootingStarActions[ShootingStarType.Quest] = _questPanel.ShowNewQuest;
        _shootingStarActions[ShootingStarType.SpecialQuest] = _questPanel.ShowNewQuest;
        _shootingStarActions[ShootingStarType.Resource] = _resourcePanel.ShowGetResource;
        _shootingStarActions[ShootingStarType.SpecialResource] = _resourcePanel.ShowGetResource;
    }

    public override void OpenPanel()
    {
        if (IsAddUIStack && !gameObject.activeSelf)
        {
            App.GetManager<UIManager>().PushUIState(UIState);
        }

        _pickBack.SetActive(true);
    }
    
    public override void ClosePanel()
    {
        if (IsAddUIStack && gameObject.activeSelf)
        {
            App.GetManager<UIManager>().PopUIState(UIState);
        }

        _pickBack.SetActive(false);
    }

    public void OpenPanel(ShootingStarType type)
    {
        OpenPanel();
        
        StartCoroutine(Harvest(type));
    }

    private IEnumerator Harvest(ShootingStarType type)
    {
        yield return new WaitForSeconds(1);

        ClosePanel();
        
        GetPickAction(type).Invoke();
    }
    
    private Action GetPickAction(ShootingStarType type)
    {
        return _shootingStarActions[type];
    }
}
