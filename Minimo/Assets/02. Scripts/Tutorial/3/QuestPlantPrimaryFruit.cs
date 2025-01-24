using UnityEngine;

public class QuestPlantPrimaryFruit : QuestBase
{
    public override string ID => "PlantPrimary_Fruit";
    protected override bool IsClear => CheckClear();

    [SerializeField] private Transform _builidngParent;
    private ProduceObject _produceObject;
    public override void StartQuest()
    {
        base.StartQuest();

        for (var i = 0; i < _builidngParent.childCount; i++) 
        {
            if (string.Equals(_builidngParent.GetChild(i).gameObject.name, "Building_Orchard(Clone)"))
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
        if (_produceObject == null) 
        {
            return false;
        }
        
        return _produceObject.AllTasks.Count > 0;
    }
}