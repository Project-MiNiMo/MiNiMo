using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceInfoCtrl : MonoBehaviour
{
    [SerializeField] Button _infoBtn;
    [SerializeField] Button _harvestBtn;
    [SerializeField] GameObject _remainTimeObj;
    [SerializeField] TextMeshProUGUI _remainTimeTMP;

    private ProduceManager _produceManager;
    
    private RectTransform _rect;
    
    private void Start()
    {
        //_infoBtn.onClick.AddListener(() => App.GetManager<GridManager>().ConfirmObject());
        //_harvestBtn.onClick.AddListener(() => App.GetManager<GridManager>().RotateObject());
        _rect = GetComponent<RectTransform>();
        
        _produceManager = App.GetManager<ProduceManager>();
        
        _produceManager.IsProducing
            .Subscribe((isProducing) =>
            {
                if (isProducing) 
                {
                    SetPosition();
                }
            }).AddTo(gameObject);
        
        _produceManager.CurrentRemainTime
            .Subscribe(SetRemainTime)
            .AddTo(gameObject);
    }
    
    private void SetPosition()
    {
        var position = _produceManager.CurrentProduceObject.transform.position;
        position.y += 3f;
        var screenPos = Camera.main.WorldToScreenPoint(position);
        _rect.position = screenPos;
    }

    private void SetRemainTime(int remainTime)
    {
        _remainTimeObj.SetActive(remainTime < 0);
        _remainTimeTMP.text = FormatTime(remainTime);
    }

    private string FormatTime(int timeInSeconds)
    {
        var minutes = timeInSeconds / 60;
        var seconds = timeInSeconds % 60;
        
        return $"{minutes:00}:{seconds:00}"; 
    }
}
