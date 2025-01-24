using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPlantPrimaryCrop : QuestBase
{
    public override string ID => "PlantPrimary_Crop";
    protected override bool IsClear => CheckClear();

    [SerializeField] private Transform _builidngParent;
    private ProduceObject _produceObject;
    public override void StartQuest()
    {
        base.StartQuest();

        for (var i = 0; i < _builidngParent.childCount; i++) 
        {
            if (string.Equals(_builidngParent.GetChild(i).gameObject.name, "Building_Farm(Clone)"))
            {
                _produceObject = _builidngParent.GetChild(i).GetComponent<ProduceObject>();
                break;
            }
        }
    }
    
    protected override void ShowDetail()
    {
        //App.GetManager<UIManager>().OpenPanel<UIQuestDetail>(this);
    }

    private bool CheckClear()
    {
        return _produceObject.AllTasks.Count > 0;
    }
}