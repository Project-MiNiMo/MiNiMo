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

    public void ShowTitle()
    {
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
