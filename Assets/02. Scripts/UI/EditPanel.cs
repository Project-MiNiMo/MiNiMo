using UnityEngine;
using UnityEngine.UI;

public class EditPanel : UIBase, IListener
{
    [SerializeField] private Button editEndBtn;

    public override void Initialize()
    {
        var eventManager = App.Instance.GetManager<EventManager>();

        eventManager.AddListener(EventCode.EditStart, this);
        eventManager.AddListener(EventCode.EditEnd, this);

        editEndBtn.onClick.AddListener(() => eventManager.PostEvent(EventCode.EditEnd, this));

        gameObject.SetActive(false);
    }

    public void OnEvent(EventCode _code, Component _sender, object _param = null)
    {
        switch (_code)
        {
            case EventCode.EditStart:
                gameObject.SetActive(true);
                break;

            case EventCode.EditEnd:
                gameObject.SetActive(false);
                break;
        }
    }
}
