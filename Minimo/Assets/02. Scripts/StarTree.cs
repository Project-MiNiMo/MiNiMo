using UnityEngine;
using UnityEngine.EventSystems;

public class StarTree : InteractObject
{
    private StarTreePanel _starTreePanel;

    private void Start()
    {
        _starTreePanel = App.GetManager<UIManager>().GetPanel<StarTreePanel>();
    }

    public override void OnLongPress() { }

    public override void OnClickUp()
    {
        _starTreePanel.OpenPanel();
    }
}