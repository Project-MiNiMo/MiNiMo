using System.Linq;

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
        _closeBtn.onClick.AddListener(()=>
        {
            _produceManager.DeactiveProduce();
        });

        _titleTMP.text = App.GetData<TitleData>().GetString(_produceManager.CurrentProduceObject.Data.Name);
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        
        InitTaskBtns();
    }

    private void InitTaskBtns()
    {
        foreach (var taskBtn in _taskBtns)
        {
            taskBtn.Initialize(_produceManager.CurrentProduceObject);
        }
    }
    
    public bool ExpandTaskBtn()
    {
        foreach (var taskBtn in _taskBtns)
        {
            if (!taskBtn.IsActive)
            {
                taskBtn.gameObject.SetActive(true);
                break;
            }
        }
        
        return  _taskBtns.Any(btn => !btn.IsActive);
    }
}
