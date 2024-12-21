using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ProduceOptionCtrl : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    private ProduceManager _produceManager;
    
    private ProduceOptionBtn[] _optionBtns;
    
    private void OnEnable()
    {
        _scrollRect.horizontalNormalizedPosition = 0;
    }

    private void Start()
    {
        _optionBtns = GetComponentsInChildren<ProduceOptionBtn>(true);
        
        _produceManager = App.GetManager<ProduceManager>();
        
        _produceManager.IsProducing
            .Subscribe((isProducing) =>
            {
                if (isProducing)
                {
                    if (_produceManager.CurrentProduceObject.ActiveTask == null 
                        && _produceManager.CurrentProduceObject.CompleteTasks.Count <= 0)
                    {
                        gameObject.SetActive(true);
                        InitOptionButtons(_produceManager.CurrentProduceObject);
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
            }).AddTo(gameObject);
    }

    private void InitOptionButtons(ProduceObject produceObject)
    {
        var options = produceObject.ProduceData.ProduceOptions;

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
