using UnityEngine;
using UnityEngine.EventSystems;

public enum MeteorType
{
    Quest,
    SpecialQuest,
    Resource,
    SpecialResource
}

public class Meteor : MonoBehaviour
{
    public bool IsLanded => gameObject.activeSelf;

    private PickMeteorPanel _pickMeteorPanel;
 
    private void Start()
    {
        _pickMeteorPanel = App.GetManager<UIManager>().GetPanel<PickMeteorPanel>();
        gameObject.SetActive(false);
    }

    public void Land(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        var type = GetRandomType();
        _pickMeteorPanel.OpenPanel(type);
    }

    private MeteorType GetRandomType()
    {
         return (MeteorType)Random.Range(0, 4);
    }
}
