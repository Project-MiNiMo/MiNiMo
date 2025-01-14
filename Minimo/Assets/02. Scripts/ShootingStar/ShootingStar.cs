using UnityEngine;
using UnityEngine.EventSystems;

public enum ShootingStarType
{
    Quest,
    SpecialQuest,
    Resource,
    SpecialResource
}

public class ShootingStar : MonoBehaviour
{
    public bool IsLanded => gameObject.activeSelf;

    private PickShootingStarPanel _pickShootingStarPanel;
 
    private void Start()
    {
        _pickShootingStarPanel = App.GetManager<UIManager>().GetPanel<PickShootingStarPanel>();
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
        _pickShootingStarPanel.OpenPanel(type);
    }

    private ShootingStarType GetRandomType()
    {
         return (ShootingStarType)UnityEngine.Random.Range(0, 4);
    }
}
