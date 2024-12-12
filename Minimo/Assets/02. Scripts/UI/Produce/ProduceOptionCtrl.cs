using UnityEngine;
using UnityEngine.UI;

public class ProduceOptionCtrl : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    
    private ProduceOptionBtn[] _optionBtns;
    private ProduceManager _produceManager;
    
    private void OnEnable()
    {
        _scrollRect.horizontalNormalizedPosition = 0;
    }

    private void Start()
    {
        _optionBtns = GetComponentsInChildren<ProduceOptionBtn>(true);
        _produceManager = App.GetManager<ProduceManager>();
    }

    public void SetActive(bool isActive) 
    {
        gameObject.SetActive(isActive);

        if (isActive) 
        {
            InitOptionButtons();
        }
    }

    private void InitOptionButtons()
    {
        var options = _produceManager.CurrentProduceObject.ProduceData.ProduceOptions;

        var i = 0;
        
        for (; i < options.Length; i++) 
        {
            var option = options[i];
            _optionBtns[i].gameObject.SetActive(true);
            _optionBtns[i].Initialize(i, option);
        }

        for (; i < _optionBtns.Length; i++) 
        {
            _optionBtns[i].gameObject.SetActive(false);
        }
    }
}
