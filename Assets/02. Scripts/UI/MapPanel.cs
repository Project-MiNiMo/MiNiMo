using UnityEngine;
using UnityEngine.UI;

public class MapPanel : UIBase, IEventListener
{
    [SerializeField] private Button _editStartBtn;

    public override void Initialize()
    {
        var eventManager = App.Instance.GetManager<EventManager>();

        eventManager.AddListener(EventCode.EditStart, this);
        eventManager.AddListener(EventCode.EditEnd, this);

        _editStartBtn.onClick.AddListener(() => eventManager.PostEvent(EventCode.EditStart, this));

        gameObject.SetActive(true);
    }

    public void OnEvent(EventCode code, Component sender, object param = null)
    {
        switch (code)
        {
            case EventCode.EditStart:
                gameObject.SetActive(false);
                break;

            case EventCode.EditEnd:
                gameObject.SetActive(true);
                break;
        }
    }
}