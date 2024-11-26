using UnityEngine;

public class StarTree : MonoBehaviour
{
    private StarTreePanel _starTreePanel;

    private void Start()
    {
        _starTreePanel = App.GetManager<UIManager>().GetPanel<StarTreePanel>();
    }

    private void OnMouseUp()
    {
        _starTreePanel.OpenPanel();
    }
}
