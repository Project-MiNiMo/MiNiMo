using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MinimoShared;
using UniRx;

public class LevelUpPanel : UIBase
{
    protected override GameObject Panel => _canvasGroup.gameObject;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _arrowImg;
    [SerializeField] private Image _arrowImg2;
    [SerializeField] private TextMeshProUGUI _levelUpText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private CanvasGroup _rewardCanvasGroup;
    [SerializeField] private GameObject _level2Reward;
    [SerializeField] private GameObject _level3Reward;
    
    public override void Initialize()
    {
        App.GetManager<AccountInfoManager>().Level
            .Subscribe((level) =>
            {
                if (level <= 1) 
                {
                    return;
                }
                var prevLevel = level - 1;
                _levelText.text = $"Lv. {prevLevel} <color=grey>>></color> <color=yellow>Lv. {level}</color>";
                SetReward(level);
                OpenPanel();
            });
        
        Setup();
        ClosePanel();
    }
    
    public override void OpenPanel()
    {
        base.OpenPanel();
        
        Setup();
        
        var seqence = DOTween.Sequence();
        seqence.Append(_canvasGroup.DOFade(1, 0.5f))
            .Append(_arrowImg.rectTransform.DOAnchorPosY(_arrowImg.rectTransform.rect.y - 20, 1).From())
            .Join(_arrowImg.DOFade(1, 1))
            .Insert(1, _arrowImg2.rectTransform.DOAnchorPosY(_arrowImg.rectTransform.rect.y - 20, 1).From())
            .Join(_arrowImg2.DOFade(1, 1))
            .Insert(1.5f,_levelUpText.DOFade(1, 1))
            .Join(_levelText.DOFade(1, 1))
            .Append(_rewardCanvasGroup.DOFade(1, 0.5f));
    }

    private void Setup()
    {
        _canvasGroup.alpha = 0;
        _arrowImg.DOFade(0, 0);
        _arrowImg2.DOFade(0, 0);
        _levelUpText.DOFade(0, 0);
        _levelText.DOFade(0, 0);
        _rewardCanvasGroup.alpha = 0;
    }

    private void SetReward(int level)
    {
        if (level == 2)
        {
            var accountInfo = App.GetManager<AccountInfoManager>();
            _level2Reward.SetActive(true);
            _level3Reward.SetActive(false);

            var newCurrency = new CurrencyDTO()
            {
                Star = accountInfo.Star.Value,
                BlueStar = 20,
            };
            accountInfo.UpdateCurrency(newCurrency);
        }
        else
        {
            _level2Reward.SetActive(false);
            _level3Reward.SetActive(true);
            
            var buildingPanel = App.GetManager<UIManager>().GetPanel<BuildingPanel>();
            buildingPanel.SetBuildingBtnForTutorial(new string[]
            {
                "Building_Farm",
                "Building_Orchard",
                "Building_CropFacility",
                "Building_OrchardFacility",
            });
        }
    }
}
