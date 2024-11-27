using UnityEngine;
using UnityEngine.EventSystems;

public class StarTree : MonoBehaviour
{
    private StarTreePanel _starTreePanel;

    private void Start()
    {
        _starTreePanel = App.GetManager<UIManager>().GetPanel<StarTreePanel>();
    }

    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        _starTreePanel.OpenPanel();
    }
}