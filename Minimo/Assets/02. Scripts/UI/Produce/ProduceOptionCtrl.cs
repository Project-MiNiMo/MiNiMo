using UniRx;
using UnityEngine;

public class ProduceOptionCtrl : MonoBehaviour
{
    private ProduceManager _produceManager;
    
    private ProduceOptionBtn[] _optionBtns;

    private void Start()
    {
        _optionBtns = GetComponentsInChildren<ProduceOptionBtn>(true);
        
        _produceManager = App.GetManager<ProduceManager>();
        
        _produceManager.IsProducing
            .Subscribe((isProducing) =>
            {
                if (isProducing)
                {
                    InitOptionButtons(_produceManager.CurrentProduceObject);
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
