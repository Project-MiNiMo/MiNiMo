using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageBtn : MonoBehaviour
{
    [SerializeField] private Image _objectImg;
    [SerializeField] private TextMeshProUGUI _objectCountTxt;

    private GameObject _objectPrefab;
    private Transform _gridObjectGroup;

    private int _objectCount;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(CreateObject);
    }

    public void Initialize(GridObjectData data, Transform gridObjectGroup)
    {
        _gridObjectGroup = gridObjectGroup;

        _objectCount = 5; 

        string spritePath = $"GridObject/Sprite/{data.Sprite}";
        string prefabPath = $"GridObject/Prefab/{data.Prefab}";

        _objectImg.sprite = Resources.Load<Sprite>(spritePath);
        _objectPrefab = Resources.Load<GameObject>(prefabPath);

        if (_objectImg.sprite == null)
        {
            Debug.LogError($"Sprite not found at path: {spritePath}");
        }

        if (_objectPrefab == null)
        {
            Debug.LogError($"Prefab not found at path: {prefabPath}");
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

        var objInstance = Instantiate(_objectPrefab, _gridObjectGroup);
        var gridObject = objInstance.GetComponentInChildren<GridObject>();

        if (gridObject != null)
        {
            AddObjectCount(-1);
            App.Instance.GetManager<GridManager>()?.SetObject(gridObject, false);
        }
        else
        {
            Debug.LogError("GridObject component not found in instantiated prefab.");
            Destroy(objInstance);
        }
    }
}
