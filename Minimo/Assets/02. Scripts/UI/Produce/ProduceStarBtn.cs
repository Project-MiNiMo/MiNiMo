using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceStarBtn : MonoBehaviour
{
    [SerializeField] private Button _starBtn;
    [SerializeField] private TextMeshProUGUI _starText;

    private ProduceManager _produceManager;
    private UseCashPanel _useCashPanel;
    
    private int _starValue;
    private int _currentStarCount;
    
    private void Start()
    {
        _useCashPanel = App.GetManager<UIManager>().GetPanel<UseCashPanel>();
        
        _starValue = App.GetData<TitleData>().Common["BlueStarValue"];

        _produceManager = App.GetManager<ProduceManager>();
        _produceManager.CurrentRemainTime
            .Subscribe(SetStarText)
            .AddTo(gameObject);
        
        _starBtn.onClick.AddListener(OnClickStarBtn);
    }

    private void OnClickStarBtn()
    {
        _useCashPanel.OpenPanel(UseCashType.Produce, 
            _currentStarCount, 
            ()=>_produceManager.HarvestEarly());
    }

    private void SetStarText(int remainTime)
    {
        _currentStarCount = (remainTime / _starValue) + 1;
        _starText.text = _currentStarCount.ToString();
    }
}
