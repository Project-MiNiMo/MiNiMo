using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrimaryPanel : UIBase
{
    [SerializeField] private ProduceHarvestCtrl _harvestCtrl;
    [SerializeField] private ProduceInfoCtrl _infoCtrl;
    [SerializeField] private RectTransform _rect;
    private ProduceManager _produceManager;
    private PlantPanel _plantPanel;

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
                    OpenPanel();
                }
                else
                {
                    ClosePanel();
                }
            }).AddTo(gameObject);

        _plantPanel = App.GetManager<UIManager>().GetPanel<PlantPanel>();
    }
    
    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _produceManager.DeactiveProduce();
        }
    }

    public override void OpenPanel()
    {
        base.OpenPanel();

        SetPosition();
        
        var currentObject = _produceManager.CurrentProduceObject;
        Debug.Log(currentObject.ActiveTaskIndex);

        if (currentObject.ActiveTaskIndex > 0)
        {
            ShowUI(_harvestCtrl);
        }
        else if (currentObject.ActiveTaskIndex >= 0)
        {
            ShowUI(_infoCtrl);
        }
        else
        {
            _plantPanel.OpenPanel();
            ShowUI(_plantPanel);
        }
    }

    private void ShowUI(MonoBehaviour targetUI)
    {
        _harvestCtrl.gameObject.SetActive(targetUI == _harvestCtrl);
        _infoCtrl.SetActive(targetUI == _infoCtrl);
    }
    
    private void SetPosition()
    {
        var position = _produceManager.CurrentProduceObject.transform.position;
        var screenPos = Camera.main.WorldToScreenPoint(position);
        _rect.position = screenPos;
    }
}
