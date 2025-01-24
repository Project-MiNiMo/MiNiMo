using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Tutorial7_SecondMeteor : TutorialBase
{
    [SerializeField] private CanvasGroup _questCanvasGroup1;
    [SerializeField] private CanvasGroup _questCanvasGroup2;
    [SerializeField] private GameObject _meteor;
    
    private SidePanel _sidePanel;

    private void Start()
    {
        _sidePanel = App.GetManager<UIManager>().GetPanel<SidePanel>();
    }
    
    public override void StartTutorial()
    {
        gameObject.SetActive(true);
        _meteor.SetActive(true);
        
        Camera.main.DOShakePosition(1, 10)
            .OnComplete(() =>
            {
                _questCanvasGroup1.DOFade(1, 0.5f)
                    .OnComplete(() => _questCanvasGroup1.DOFade(0, 0.5f));
            });
        
        StartCoroutine(WaitForMeteor());
    }
    
    private IEnumerator WaitForMeteor()
    {
        var questManager = App.GetManager<QuestManager>();
        yield return new WaitUntil(() => questManager.CurrentQuestID == "SecondMeteor");
   
        _questCanvasGroup2.DOFade(1, 0.5f);

        StartCoroutine(WaitUntilSideOpen());
    }

    private IEnumerator WaitUntilSideOpen()
    {
        yield return new WaitUntil(() => _sidePanel.IsOpen);
        
        _questCanvasGroup2.DOFade(0, 0.5f)
            .OnComplete(() => _questCanvasGroup2.gameObject.SetActive(false));
    }
    
    public override void EndTutorial()
    {
        _sidePanel.Close(); 
        gameObject.SetActive(false);
    }
}
