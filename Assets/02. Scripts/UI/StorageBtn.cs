using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageBtn : MonoBehaviour
{
    [SerializeField] private Image _objectImg;
    [SerializeField] private TextMeshProUGUI _objectCountTxt;

    private BuildingData _data;
    private GameObject _objectPrefab;
    private Transform _buildingGroup;

    private int _objectCount;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(CreateObject);

        string prefabPath = $"Building/GridObject";
        _objectPrefab = Resources.Load<GameObject>(prefabPath);
    }

    public void Initialize(BuildingData data, Transform gridObjectGroup)
    {
        _data = data;

        _buildingGroup = gridObjectGroup;

        _objectCount = 5; 

        string spritePath = $"Building/Icon/{data.Icon}";
        _objectImg.sprite = Resources.Load<Sprite>(spritePath);
        _objectImg.SetNativeSize();

        if (App.Instance.GetData<PlayerData>().PlayerLevel < data.UnlockLevel)
        {
            GetComponent<Button>().interactable = false;
        }

        UpdateObjectCountText();
    }

    public void AddObjectCount(int count)
    {
        _objectCount += count;
        UpdateObjectCountText();
    }

    private void UpdateObjectCountText()
    {
        _objectCountTxt.text = _objectCount.ToString();
    }

    private void CreateObject()
    {
        if (_objectCount <= 0)
        {
            Debug.LogWarning("No objects left to create.");
            return;
        }

        var gridObject = Instantiate(_objectPrefab, _buildingGroup).GetComponentInChildren<GridObject>();

        if (gridObject != null)
        {
            AddObjectCount(-1);

            gridObject.Initialize(_data, _objectImg.sprite);
            App.Instance.GetManager<GridManager>().SetObject(gridObject, false);
        }
        else
        {
            Debug.LogError("GridObject component not found in instantiated prefab.");
            Destroy(gridObject.gameObject);
        }
    }
}
