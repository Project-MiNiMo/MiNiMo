using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : ManagerBase
{
    [SerializeField] private List<QuestBase> _quests;
    [SerializeField] private Meteor _firstMeteor;

    private int _installPrimaryCount = 0;
    private int _plantPrimaryCount = 0;
    private int _harvestPrimaryCount = 0;
    private int _installAdvancedCount = 0;
    private SidePanel _sidePanel;

    private void Start()
    {
        _sidePanel = App.GetManager<UIManager>().GetPanel<SidePanel>();
        _firstMeteor.enabled = false;
    }

    public void StartQuest(string id)
    {
        _sidePanel.SetAlert(true);
        
        var quest = _quests.Find(q => q.ID == id);
        quest.StartQuest();
    }
    
    public void EndQuest(string id)
    {
        _sidePanel.SetAlert(true);
        
        switch (id)
        {
            case "InstallPrimary_Crop":
                _installPrimaryCount++;
                if (_installPrimaryCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
            case "InstallPrimary_Fruit":
                _installPrimaryCount++;
                if (_installPrimaryCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
            
            case "PlantPrimary_Crop":
                _plantPrimaryCount++;
                if (_plantPrimaryCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
            case "PlantPrimary_Fruit":
                _plantPrimaryCount++;
                if (_plantPrimaryCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
            
            case "HarvestPrimary_Crop":
                _harvestPrimaryCount++;
                if (_harvestPrimaryCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
            case "HarvestPrimary_Fruit":
                _harvestPrimaryCount++;
                if (_harvestPrimaryCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
            
            case "FirstMeteor":
                App.GetManager<TutorialManager>().NextTutorial();
                break;
            
            case "InstallAdvanced_Crop":
                _installAdvancedCount++;
                if (_installAdvancedCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
            case "InstallAdvanced_Fruit":
                _installAdvancedCount++;
                if (_installAdvancedCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
        }
    }
}
