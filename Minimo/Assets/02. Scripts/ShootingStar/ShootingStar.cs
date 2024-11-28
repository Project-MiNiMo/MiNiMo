using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingStar : MonoBehaviour
{
    private enum EShootingStar
    {
        GreenSS,
        BlueSS,
        YellowSS,
        RainbowSS
    }

    private EShootingStar _shootingStarType;

    private ShootingStarCtrl _shootingStarCtrl;
    private HarvestShootingStarPanel _harvestShootingStarPanel;

    public void Initialize(ShootingStarCtrl shootingStarCtrl)
    {
        _shootingStarCtrl = shootingStarCtrl;

        _harvestShootingStarPanel = App.GetManager<UIManager>().GetPanel<HarvestShootingStarPanel>();
    }

    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        StartCoroutine(HarvestShootingStar());
    }

    private IEnumerator HarvestShootingStar()
    {
        SetRandomShootingStarType();

        _harvestShootingStarPanel.OpenPanel();

        yield return new WaitForSeconds(1);

        _harvestShootingStarPanel.ClosePanel();

        ShootingStarAction();

        _shootingStarCtrl.OnHarvestShootingStar(gameObject);
    }

    private void SetRandomShootingStarType()
    {
        _shootingStarType = (EShootingStar)Random.Range(0, 4);
    }

    private void ShootingStarAction()
    {
        switch (_shootingStarType)
        {
            case EShootingStar.GreenSS:
            case EShootingStar.BlueSS:
                App.GetManager<UIManager>().GetPanel<QuestPanel>().ShowNewQuest();
                break;

            case EShootingStar.YellowSS:
            case EShootingStar.RainbowSS:
                App.GetManager<UIManager>().GetPanel<ResourcePanel>().ShowGetResource();
                break;
        }
    }
}
