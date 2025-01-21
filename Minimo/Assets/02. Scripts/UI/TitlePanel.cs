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
    
    [SerializeField] private Image _blackBlur;
     
    private void Awake()
    {
        _startBtn.onClick.AddListener(OnClickStart);
        _startBtn.gameObject.SetActive(false);
    }

    private void Start()
    {
        AkSoundEngine.PostEvent("BGM_Title", gameObject);
    }

    public async void ShowTitle()
    {
        App.GetManager<CheatManager>().UpdateItem(new ItemDTO {ItemType = "Item_Timber", Count = 10}).Forget();
        App.GetManager<CheatManager>().UpdateCurrency(new CurrencyDTO {HPI = 100, BlueStar = 100}).Forget();
        
        foreach (var building in App.GetData<TitleData>().Building.Values)
        {
            if (App.GetManager<AccountInfoManager>().Level >= building.UnlockLevel)
            {
                await App.GetManager<CheatManager>().UpdateBuildingInfo(new BuildingInfoDTO()
                {
                    BuildingType = building.ID,
                    MaxCount = 5,
                    ProduceSlotCount = 3,
                });
            }
        }
        
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
                App.LoadScene(SceneName.Game);
            });
    }
}
