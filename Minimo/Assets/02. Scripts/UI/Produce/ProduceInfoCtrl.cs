using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceInfoCtrl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoTMP;
    [SerializeField] private Image[] _infoImages;
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
                    if (_produceManager.CurrentProduceObject.ActiveTask != null)
                    {
                        gameObject.SetActive(true);
                        _currentOption = _produceManager.CurrentProduceObject.ActiveTask.Data;
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
        _infoTMP.text = string.Empty;
        
        var i = 0;
        
        for (; i < _currentOption.Results.Length; i++) 
        {
            if (!_titleData.Item.TryGetValue(_currentOption.Results[i].Code, out var itemData))
            {
                Debug.LogError($"Cannot find item data with code : {_currentOption.Results[i].Code}");
                return;
            }

            if (i > 0) 
            {
                _infoTMP.text += " / ";
            }
            
            _infoTMP.text += 
                $"{_titleData.GetString(itemData.Name)} X{_currentOption.Results[i].Amount}";
            _infoImages[i].sprite = Resources.Load<Sprite>($"Item/{itemData.ID}");
        }

        for (; i < _infoImages.Length; i++) 
        {
            _infoImages[i].gameObject.SetActive(false);
        }
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
