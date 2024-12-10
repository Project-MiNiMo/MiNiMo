using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceInfoCtrl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoTMP;
    [SerializeField] private Image _infoImage;
    [SerializeField] private Button _starBtn;
    [SerializeField] private Image _remainTimeImg;
    [SerializeField] private TextMeshProUGUI _remainTimeTMP;

    private ProduceManager _produceManager;
    private TitleData _titleData;
    
    private RectTransform _rect;
    private ProduceOption _currentOption;
    
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        
        _produceManager = App.GetManager<ProduceManager>();
        _titleData = App.GetData<TitleData>();
        
        //_starBtn.onClick.AddListener(() => App.GetManager<GridManager>().RotateObject());
        
        _produceManager.IsProducing
            .Subscribe((isProducing) =>
            {
                if (isProducing) 
                {
                    if (_produceManager.CurrentProduceObject.CurrentState == ProduceState.Produce)
                    {
                        gameObject.SetActive(true);
                        _currentOption = _produceManager.CurrentProduceObject.CurrentOption;
                        SetPosition();
                        SetInfo();
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }

                }
            }).AddTo(gameObject);
        
        _produceManager.CurrentRemainTime
            .Subscribe(SetRemainTime)
            .AddTo(gameObject);
    }
    
    private void SetPosition()
    {
        var position = _produceManager.CurrentProduceObject.transform.position;
        var screenPos = Camera.main.WorldToScreenPoint(position);
        _rect.position = screenPos;
    }

    private void SetInfo()
    {
        if (!_titleData.Item.TryGetValue(_currentOption.Results[0].Code, out var itemData))
        {
            Debug.LogError($"Cannot find item data with code : {_currentOption.Results[0].Code}");
            return;
        }
        
        _infoTMP.text = 
            $"{_titleData.GetString(itemData.Name)} X{_currentOption.Results[0].Amount}";
        _infoImage.sprite = Resources.Load<Sprite>($"Item/{itemData.ID}");
    }

    private void SetRemainTime(int remainTime)
    {
        if (_currentOption == null)
        {
            return;
        }

        if (remainTime <= 0)
        {
            gameObject.SetActive(false);
        }

        _remainTimeTMP.text = FormatTime(remainTime);
        _remainTimeImg.fillAmount = 1 - ((float)remainTime / _currentOption.Time);
    }

    private string FormatTime(int timeInSeconds)
    {
        var hours = timeInSeconds / 3600;
        var minutes = (timeInSeconds % 3600) / 60;
        var seconds = timeInSeconds % 60;
    
        return $"{hours:00}:{minutes:00}:{seconds:00}"; 
    }
}
