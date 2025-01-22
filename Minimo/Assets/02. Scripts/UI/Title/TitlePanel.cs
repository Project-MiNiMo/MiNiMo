using MinimoShared;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitlePanel : MonoBehaviour
{
    [SerializeField] private Button _startBtn;
    [SerializeField] private RectTransform _startTextRect;
    [SerializeField] private Image _titleFstImg;
    [SerializeField] private Image _titleSndImg;
    
    [SerializeField] private TitleLoadHandler _loadHandler;
    
    [SerializeField] private Image _blackBlur;

    private bool _isNew;
    
    private void Awake()
    {
        _startBtn.onClick.AddListener(OnClickStart);
        _startBtn.gameObject.SetActive(false);
    }

    public async void ShowTitle(bool isNew = false)
    {
        _isNew = isNew;
        _loadHandler.Setup(3 + App.GetData<TitleData>().Building.Values.Count);
        
        App.GetManager<CheatManager>().UpdateItem(new ItemDTO {ItemType = "Item_Timber", Count = 10}).Forget();
        _loadHandler.UpdateLoad();
        App.GetManager<CheatManager>().UpdateCurrency(new CurrencyDTO {HPI = 100, BlueStar = 100}).Forget();
        _loadHandler.UpdateLoad();
        
        foreach (var building in App.GetData<TitleData>().Building.Values)
        {
            if (App.GetManager<AccountInfoManager>().Level >= building.UnlockLevel)
            { 
                var produceSlotCount = building.ID == "Building_Orchard" ? 5 : 3;
                await App.GetManager<CheatManager>().UpdateBuildingInfo(new BuildingInfoDTO()
                {
                    BuildingType = building.ID,
                    MaxCount = 5,
                    ProduceSlotCount = produceSlotCount,
                });
                
                _loadHandler.UpdateLoad();
            }
        }
        
        _loadHandler.FinishLoad();
        _startBtn.gameObject.SetActive(true);
        
        _startTextRect.DOJumpAnchorPos(Vector2.one, 10, 1, 1)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    private void OnClickStart()
    {
        _startTextRect.DOKill();
        _startBtn.gameObject.SetActive(false);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_titleFstImg.DOFade(0, 1))
            .Join(_titleSndImg.DOFade(1, 1))
            .AppendCallback(() => _blackBlur.gameObject.SetActive(true))
            .Append(_blackBlur.DOFade(1, 0.5f))
            .OnComplete(() =>
            {
                App.LoadScene(_isNew? SceneName.Prolog : SceneName.Game);
            });
    }
}
