using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.EventSystems;

public class Meteor : MonoBehaviour
{
    public bool IsLanded => gameObject.activeSelf;

    private MeteorManager _meteorManager;
    private PickMeteorPanel _pickMeteorPanel;

    private int _id;
 
    private void Start()
    {
        _meteorManager = App.GetManager<MeteorManager>();
        _pickMeteorPanel = App.GetManager<UIManager>().GetPanel<PickMeteorPanel>();
        gameObject.SetActive(false);
    }

    public void Land(int id, Vector3 position)
    {
        _id = id;
        
        position += new Vector3(0, 0.25f, 0);
        transform.position = position;
        gameObject.SetActive(true);
    }

    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        PickMeteor();
    }

    private async void PickMeteor()
    {
        var result = await _meteorManager.GetMeteorResult(_id);

        if (result.IsSuccess) 
        {
            _pickMeteorPanel.OpenPanel(result.Data);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Failed to pick meteor");
        }
    }
}
