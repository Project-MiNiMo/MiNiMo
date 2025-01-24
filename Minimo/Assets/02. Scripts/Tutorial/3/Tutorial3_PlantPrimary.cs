using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Tutorial3_PlantPrimary : TutorialBase
{
    [SerializeField] private CanvasGroup _questCanvasGroup;
    
    private SidePanel _sidePanel;

    private void Start()
    {
        _sidePanel = App.GetManager<UIManager>().GetPanel<SidePanel>();
    }
    
    public override void StartTutorial()
    {
        gameObject.SetActive(true);
        _questCanvasGroup.DOFade(1, 0.5f);

        App.GetManager<QuestManager>().StartQuest("PlantPrimary_Crop");
        App.GetManager<QuestManager>().StartQuest("PlantPrimary_Fruit");
        
        StartCoroutine(WaitUntilSideOpen());
    }

    private IEnumerator WaitUntilSideOpen()
    {
        yield return new WaitUntil(() => _sidePanel.IsOpen);
        
        _questCanvasGroup.DOFade(0, 0.5f)
            .OnComplete(() => _questCanvasGroup.gameObject.SetActive(false));
    }
    
    public override void EndTutorial()
    {
        gameObject.SetActive(false);
    }
}
