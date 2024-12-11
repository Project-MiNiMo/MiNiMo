using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceStarBtn : MonoBehaviour
{
    [SerializeField] private Button _starBtn;
    [SerializeField] private TextMeshProUGUI _starText;

    private ProduceManager _produceManager;
    private int _starValue;
    
    private void Start()
    {
        _starValue = App.GetData<TitleData>().Common["BlueStarValue"];

        _produceManager = App.GetManager<ProduceManager>();
        _produceManager.CurrentRemainTime
            .Subscribe(SetStarText)
            .AddTo(gameObject);
        
        _starBtn.onClick.AddListener(OnClickStarBtn);
    }

    //TODO : Check And Use BlueStar
    private void OnClickStarBtn()
    {
        _produceManager.HarvestEarly();
    }

    private void SetStarText(int remainTime)
    {
        var blueStarCount = (remainTime / _starValue) + 1;
        _starText.text = blueStarCount.ToString();
    }
}
