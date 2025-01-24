using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : ManagerBase
{
    [SerializeField] private List<QuestBase> _quests;

    private int _primaryQuestCount = 0;
    private SidePanel _sidePanel;

    private void Start()
    {
        _sidePanel = App.GetManager<UIManager>().GetPanel<SidePanel>();
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
                _primaryQuestCount++;
                if (_primaryQuestCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
            
            case "InstallPrimary_Farm":
                _primaryQuestCount++;
                if (_primaryQuestCount == 2)
                {
                    App.GetManager<TutorialManager>().NextTutorial();
                }
                break;
        }
    }
}
