using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitlePanel : MonoBehaviour
{
    [SerializeField] private Button _startBtn;
    [SerializeField] private Image _titleFstImg;
    [SerializeField] private Image _titleSndImg;
    
    private void Awake()
    {
        _startBtn.onClick.AddListener(OnClickStart);
        _startBtn.gameObject.SetActive(false);
    }

    public void ShowTitle()
    {
        _startBtn.gameObject.SetActive(true);
    }

    private void OnClickStart()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_titleFstImg.DOFade(0, 1))
            .Join(_titleSndImg.DOFade(1, 1))
            .OnComplete(() =>
            {
                App.LoadScene(SceneName.Game);
            });
    }
}
