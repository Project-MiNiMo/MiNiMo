using UnityEngine;
using UnityEngine.UI;

public class EditPanel : UIBase, IEventListener
{
    [SerializeField] private Button _editEndBtn;

    public override void Initialize()
    {
        var eventManager = App.GetManager<EventManager>();

        eventManager.AddListener(EventCode.EditStart, this);
        eventManager.AddListener(EventCode.EditEnd, this);

        _editEndBtn.onClick.AddListener(() => eventManager.PostEvent(EventCode.EditEnd, this));

        gameObject.SetActive(false);
    }

    public void OnEvent(EventCode _code, Component _sender, object _param = null)
    {
        switch (_code)
        {
            case EventCode.EditStart:
                OpenPanel();
                break;

            case EventCode.EditEnd:
                ClosePanel();
                break;
        }
    }
}
