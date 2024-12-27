using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdvancedPanel : UIBase
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private ProduceTaskBtn[] _taskBtns;

    private ProduceManager _produceManager;
    
    public override void Initialize()
    {
        _produceManager = App.GetManager<ProduceManager>();
        
        _produceManager.IsProducing
            .Subscribe((isProducing) =>
            {
                if (!isProducing)
                {
                    ClosePanel();
                    return;
                }
                
                var id = _produceManager.CurrentProduceObject.Data.ID;
                if (string.Equals(id, "Building_Farm") 
                    || string.Equals(id, "Building_Orchard"))
                {
                    ClosePanel();
                }
                else
                {
                    OpenPanel();
                }
            }).AddTo(gameObject);
        
        _taskBtns = GetComponentsInChildren<ProduceTaskBtn>(true);
        _closeBtn.onClick.AddListener(ClosePanel);
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        
        InitTaskBtns();
    }

    private void InitTaskBtns()
    {
        int i = 0;
        foreach (var task in _produceManager.CurrentProduceObject.AllTasks)
        {
            _taskBtns[i].Initialize(task);
            i++;
        }
    }
}
