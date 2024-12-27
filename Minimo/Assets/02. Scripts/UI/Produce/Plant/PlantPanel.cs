using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlantPanel : UIBase
{
    [SerializeField] private ScrollRect _scrollRect;
    
    private PlantOptionSlot[] _plantOptionSlots;
    private ProduceManager _produceManager;

    public override void Initialize()
    {
        _plantOptionSlots = GetComponentsInChildren<PlantOptionSlot>(true);
        _produceManager = App.GetManager<ProduceManager>();

        foreach (var slot in _plantOptionSlots)
        {
            slot.Start();
        }
    }
    
    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            gameObject.SetActive(false);
        }
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        
        InitOptionButtons();
        _scrollRect.horizontalNormalizedPosition = 0;
    }

    private void InitOptionButtons()
    {
        var options = _produceManager.CurrentProduceObject.ProduceData.ProduceOptions;

        var i = 0;
        
        for (; i < options.Length; i++) 
        {
            var option = options[i];
            _plantOptionSlots[i].gameObject.SetActive(true);
            _plantOptionSlots[i].Initialize(option);
        }

        for (; i < _plantOptionSlots.Length; i++) 
        {
            _plantOptionSlots[i].gameObject.SetActive(false);
        }
    }
}
