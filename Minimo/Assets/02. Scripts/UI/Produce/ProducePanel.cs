using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProducePanel : UIBase, IEventListener
{
    [SerializeField] private ProduceCircleCtrl _circleCtrl;
    [SerializeField] private ProduceItemCtrl _itemCtrl;

    public override void Initialize()
    {
        App.GetManager<EventManager>().AddListener(EventCode.EditStart, this);

        _circleCtrl.Initialize();
        _itemCtrl.Initialize();

        gameObject.SetActive(false);
    }

    public void OnEvent(EventCode _code, Component _sender, object _param = null)
    {
        switch (_code)
        {
            case EventCode.EditStart:
                ClosePanel();
                break;
        }
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            ClosePanel();
        }
    }

    public void StartManageBuilding(BuildingObject gridObject)
    {
        OpenPanel();

        _circleCtrl.SetRemainTime(false);
        _circleCtrl.SetPosition(gridObject.transform);
        _itemCtrl.InitItemButtons(gridObject);
    }

    public void StartManageBuildingOnProduce(BuildingObject gridObject, int remainTime)
    {
        OpenPanel();

        _circleCtrl.SetRemainTime(true, remainTime);
        _circleCtrl.SetPosition(gridObject.transform);
    }
}