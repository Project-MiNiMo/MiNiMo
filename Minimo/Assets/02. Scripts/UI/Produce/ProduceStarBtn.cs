using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceStarBtn : MonoBehaviour
{
    [SerializeField] private Button _starBtn;
    [SerializeField] private TextMeshProUGUI _starText;

    private ProduceManager _produceManager;
    private DiamondPanel _diamondPanel;
    
    private int _starValue;
    private int _currentStarCount;
    
    private void Start()
    {
        _diamondPanel = App.GetManager<UIManager>().GetPanel<DiamondPanel>();
        
        _starValue = App.GetData<TitleData>().Common["BlueStarValue"];

        _produceManager = App.GetManager<ProduceManager>();
        _produceManager.CurrentRemainTime
            .Subscribe(SetStarText)
            .AddTo(gameObject);
        
        _starBtn.onClick.AddListener(OnClickStarBtn);
    }

    private void OnClickStarBtn()
    {
        _diamondPanel.OpenPanel(UseDiamondType.Produce, 
            _currentStarCount, 
            ()=>_produceManager.HarvestEarly());
    }

    private void SetStarText(int remainTime)
    {
        _currentStarCount = (remainTime / _starValue) + 1;
        _starText.text = _currentStarCount.ToString();
    }
}
