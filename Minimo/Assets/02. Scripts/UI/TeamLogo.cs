using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TeamLogo : MonoBehaviour
{
    [SerializeField] private Image _logoImg;
    
    private void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(0.5f)
            .Append(_logoImg.DOFade(1, 2))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                App.LoadScene(SceneName.Title);
            });
    }
}
