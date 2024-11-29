using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProduceItemBtn : MonoBehaviour
{
    [SerializeField] private Image _objectImg;

    private BuildingObject _gridObject;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartProduce);
    }

    public void Initialize(BuildingObject gridObject, string itemID)
    {
        _gridObject = gridObject;

        if (!App.GetData<TitleData>().Item.TryGetValue(itemID, out var itemData))
        {
            return;
        }

        string spritePath = $"Item/{itemData.Icon}";
        _objectImg.sprite = Resources.Load<Sprite>(spritePath);
    }

    private void StartProduce()
    {
        if (_gridObject == null)
        {
            Debug.LogError("GridObject component not found.");
            return;
        }
        else
        {
            App.GetManager<UIManager>().GetPanel<ProducePanel>().ClosePanel();
        }
    }
}

