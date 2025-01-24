using UnityEngine;
using DG.Tweening;
using TMPro;

public class Tutorial1_SetInfo : TutorialBase
{
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private TMP_InputField _wishInput;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _minimoImgRect;
    [SerializeField] private RectTransform _minimoRect;
    [SerializeField] private RectTransform _nicknameRect;
    [SerializeField] private RectTransform _wishRect;
    [SerializeField] private RectTransform _backRect;
    [SerializeField] private RectTransform _minimoImgRect2;
    [SerializeField] private GameObject _giggleImg1;
    [SerializeField] private GameObject _giggleImg2;

    [SerializeField] private float[] _backRectYPos;
    private int _currentStep = 0;
    
    private void Start()
    {
        _nicknameInput.onSubmit.AddListener(OnUpdateNickname);
        _wishInput.onSubmit.AddListener(OnUpdateWish);
    }
    
    public override void StartTutorial()
    {
        gameObject.SetActive(true);

        var sequence = DOTween.Sequence();
        sequence.Append(_canvasGroup.DOFade(1, 1f))
            .AppendInterval(1)
            .Append(_minimoImgRect.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack))
            .Join(_backRect.DOAnchorPosY(_backRectYPos[_currentStep++], 0.5f).SetEase(Ease.OutBack))
            .Append(_minimoRect.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack))
            .AppendInterval(1)
            .Append(_nicknameRect.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack))
            .Join(_backRect.DOAnchorPosY(_backRectYPos[_currentStep++], 0.5f).SetEase(Ease.OutBack));
    }
    
    private void OnUpdateNickname(string nickname)
    {
        App.GetManager<AccountInfoManager>().UpdateNickname(nickname);
        _nicknameInput.enabled = false;
        
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(0.5f)
            .Append(_wishRect.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack))
            .Join(_backRect.DOAnchorPosY(_backRectYPos[_currentStep++], 0.5f).SetEase(Ease.OutBack));
    }
    
    private void OnUpdateWish(string wish)
    {
        App.GetManager<TutorialManager>().Wish = wish;
        _wishInput.enabled = false;

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(0.5f)
            .Append(_minimoImgRect2.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack))
            .Join(_backRect.DOAnchorPosY(_backRectYPos[_currentStep++], 0.5f).SetEase(Ease.OutBack))
            .AppendCallback(() =>
            {
                _giggleImg1.SetActive(true);
                _giggleImg2.SetActive(false);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                _giggleImg1.SetActive(false);
                _giggleImg2.SetActive(true);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                _giggleImg1.SetActive(true);
                _giggleImg2.SetActive(false);
            })
            .AppendInterval(0.5f)
            .Append(_canvasGroup.DOFade(0, 1))
            .OnComplete(EndTutorial);
    }
    
    public override void EndTutorial()
    {
        gameObject.SetActive(false);
    }
}
