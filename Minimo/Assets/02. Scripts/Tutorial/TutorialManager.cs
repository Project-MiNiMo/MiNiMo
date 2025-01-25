using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : ManagerBase
{
    [SerializeField] private List<TutorialBase> _tutorials;
    [SerializeField] private Transform[] _circleTransforms;
    [SerializeField] private Transform _endTransform;
    [SerializeField] private Image _lastImg;
    [SerializeField] private TextMeshProUGUI _wishText1;
    [SerializeField] private TextMeshProUGUI _wishText2;

    public string Wish;

    private void Start()
    {
        _tutorials[0].StartTutorial();
    }
    
    public void NextTutorial()
    {
        _tutorials[0].EndTutorial();
        _tutorials.RemoveAt(0);

        StartCoroutine(NextTutorialCoroutine());
    }
    
    private IEnumerator NextTutorialCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (_tutorials.Count > 0)
        {
            _tutorials[0].StartTutorial();
        }
        else
        {
            StartCoroutine(EndTutorial());
        }
    }

    private IEnumerator EndTutorial()
    {
        _wishText1.text = $"{Wish}....";
        foreach (var circle in _circleTransforms)
        {
            circle.DOMove(_endTransform.position, 1f);

            yield return new WaitForSeconds(0.3f);
        }
        
        var sequence = DOTween.Sequence();
        sequence.Append(_lastImg.DOFade(1, 1f))
            .AppendInterval(1f)
            .Append(_wishText1.DOFade(1, 1f))
            .Append(_wishText1.DOFade(1, 1))
            .AppendInterval(2f)
            .OnComplete(() =>
            {
                App.GetManager<UIManager>().FadeIn(() =>
                {
                    App.GetManager<LoginManager>().Logout();
                    App.LoadScene(SceneName.Title);
                });
            });
    }
}
