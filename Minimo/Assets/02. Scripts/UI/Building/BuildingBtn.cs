using System;
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

    [Serializable]
    public struct RequireItem
    {
        public GameObject ItemBack;
        public TextMeshProUGUI CountTMP;
        public Image IconImg;
    }

    [SerializeField] private Button _editBtn;
    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _countTMP;
    [SerializeField] private TextMeshProUGUI _hpiTMP;
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
    }

    public void Initialize(BuildingData data, Transform gridObjectGroup)
    {
        _data = data;

        _buildingGroup = gridObjectGroup;

        string spritePath = $"Building/Icon/{data.Icon}";
        _iconImg.sprite = Resources.Load<Sprite>(spritePath);
        _iconImg.SetNativeSize();
        
        string prefabPath = $"Building/{data.ID}";
        _objectPrefab = Resources.Load<GameObject>(prefabPath);

        SetString();
        SetRequireItem();
        SetBuildingState();
    }

    private void SetString()
    {
        var titleData = App.GetData<TitleData>();

        _nameTMP.text = titleData.GetString(_data.Name);
        _lockNoticeTMP.text = titleData.GetFormatString("STR_BUILDING_UI_LOCK", _data.UnlockLevel.ToString());
        _unlockNoticeTMP.text = titleData.GetString("STR_BUILDING_UI_UNLOCKABLE");

        _hpiTMP.text = _data.HPI.ToString();
    }

    private void SetRequireItem()
    {
        if (!App.GetData<TitleData>().Construct.TryGetValue(_data.ID, out var constructData))
        {
            Debug.LogError($"Can't Find ConstructData with ID : {_data.ID}");
            return;
        }

        if (string.IsNullOrEmpty(constructData.MatCode1))
        {
            _requireItem1.ItemBack.SetActive(false);
        }
        else
        {
            _requireItem1.ItemBack.SetActive(true);

            string spritePath = $"Item/Icon/{constructData.MatCode1}";
            _requireItem1.IconImg.sprite = Resources.Load<Sprite>(spritePath);

            _requireItem1.CountTMP.text = constructData.MatAmount1.ToString();
        }

        if (string.IsNullOrEmpty(constructData.MatCode2))
        {
            _requireItem2.ItemBack.SetActive(false);
        }
        else
        {
            _requireItem2.ItemBack.SetActive(true);

            string spritePath = $"Item/Icon/{constructData.MatCode2}";
            _requireItem2.IconImg.sprite = Resources.Load<Sprite>(spritePath);

            _requireItem2.CountTMP.text = constructData.MatAmount2.ToString();
        }
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
        App.GetManager<UIManager>().GetPanel<BuildingPanel>().ClosePanel();

        var gridObject = Instantiate(_objectPrefab, _buildingGroup).GetComponentInChildren<BuildingObject>();

        if (gridObject != null)
        {
            gridObject.Initialize(_data, _iconImg.sprite);
            App.GetManager<EditManager>().StartEdit(gridObject);
        }
        else
        {
            Debug.LogError("GridObject component not found in instantiated prefab.");
            Destroy(gridObject.gameObject);
        }
    }
}
