using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingBtn : MonoBehaviour
{
    private enum BuildingState
    {
        Use,
        Unlock,
        Lock,
    }

    public struct RequireItem
    {
        public GameObject ItemBack;
        public TextMeshProUGUI CountTMP;
        public Image IconImg;
    }

    [SerializeField] private Button _editBtn;
    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _countTMP;
    [SerializeField] private Image _iconImg;

    [SerializeField] private GameObject[] _stateBacks;

    [SerializeField] private RequireItem _requireItem1;
    [SerializeField] private RequireItem _requireItem2;

    [SerializeField] private TextMeshProUGUI _lockNoticeTMP;
    [SerializeField] private TextMeshProUGUI _unlockNoticeTMP;

    private BuildingData _data;
    private GameObject _objectPrefab;
    private Transform _buildingGroup;

    private BuildingState _currentState;

    private void Start()
    {
        _editBtn.onClick.AddListener(CreateObject);

        string prefabPath = $"Building/GridObject";
        _objectPrefab = Resources.Load<GameObject>(prefabPath);
    }

    public void Initialize(BuildingData data, Transform gridObjectGroup)
    {
        _data = data;

        _buildingGroup = gridObjectGroup;

        _nameTMP.text = App.GetData<TitleData>().GetString(data.Name);

        string spritePath = $"Building/Icon/{data.Icon}";
        _iconImg.sprite = Resources.Load<Sprite>(spritePath);
        _iconImg.SetNativeSize();

        SetBuildingState();
    }

    private void SetBuildingState()
    {
        if (App.GetData<PlayerData>().PlayerLevel < _data.UnlockLevel)
        {
            _currentState = BuildingState.Lock;
        }
        else
        {
            _currentState = BuildingState.Use;
        }

        for (int i = 0; i < _stateBacks.Length; i++)
        {
            if (i == (int)_currentState)
            {
                _stateBacks[i].SetActive(true);
            }
            else
            {
                _stateBacks[i].SetActive(false);
            }
        }
    }

    private void CreateObject()
    {
        var gridObject = Instantiate(_objectPrefab, _buildingGroup).GetComponentInChildren<GridObject>();

        if (gridObject != null)
        {
            gridObject.Initialize(_data, _iconImg.sprite);
            App.GetManager<GridManager>().SetObject(gridObject, false);
        }
        else
        {
            Debug.LogError("GridObject component not found in instantiated prefab.");
            Destroy(gridObject.gameObject);
        }
    }
}
