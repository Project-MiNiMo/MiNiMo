using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageBtn : MonoBehaviour
{
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectCountText;

    private GridObjectData data;
    private GameObject objectPrefab;
    private Transform gridObjectGroup;

    private int objectCount;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(CreateObject);
    }

    public void Initialize(GridObjectData data, Transform gridObjectGroup)
    {
        this.data = data;
        this.gridObjectGroup = gridObjectGroup;

        objectCount = 5; 

        string spritePath = $"GridObject/Sprite/{data.Sprite}";
        string prefabPath = $"GridObject/Prefab/{data.Prefab}";

        objectImage.sprite = Resources.Load<Sprite>(spritePath);
        objectPrefab = Resources.Load<GameObject>(prefabPath);

        if (objectImage.sprite == null)
        {
            Debug.LogError($"Sprite not found at path: {spritePath}");
        }

        if (objectPrefab == null)
        {
            Debug.LogError($"Prefab not found at path: {prefabPath}");
        }

        UpdateObjectCountText();
    }

    public void AddObjectCount(int count)
    {
        objectCount += count;
        UpdateObjectCountText();
    }

    private void UpdateObjectCountText()
    {
        objectCountText.text = objectCount.ToString();
    }

    private void CreateObject()
    {
        if (objectCount <= 0)
        {
            Debug.LogWarning("No objects left to create.");
            return;
        }

        var objInstance = Instantiate(objectPrefab, gridObjectGroup);
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

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
