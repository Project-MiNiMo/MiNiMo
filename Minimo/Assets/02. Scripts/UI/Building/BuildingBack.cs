using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BuildingBack : MonoBehaviour
{
    private enum EBuildingType
    {
        Production,
        Decoration,
        Utility,
        Minimo
    }

    [SerializeField] private EBuildingType _buildingType;
    [SerializeField] private Transform _buildingBtnParent;
    [SerializeField] private GameObject _buildingBtnPrefab;

    [Header("Scroll")]
    [SerializeField] private GameObject _scrollUpObj;
    [SerializeField] private GameObject _scrollDownObj;

    private Dictionary<string, BuildingBtn> _btnDictionary = new();
    private Transform _buildingObjectParent;

    private ScrollRect _scrollRect;
    private RectTransform _contentRect;
    private RectTransform _viewportRect;

    private void Start()
    {
        _buildingObjectParent = GameObject.FindWithTag("BuildingObjectParent").transform;

        _scrollRect = GetComponent<ScrollRect>();
        _contentRect = _scrollRect.content;
        _viewportRect = _scrollRect.viewport;

        InitBuildingBtns();
    }

    private void OnEnable()
    {
        if (_scrollRect == null) return;

        _scrollRect.verticalNormalizedPosition = 1;
    }

    private void InitBuildingBtns()
    {
        BuildingBtn buildingBtn;

        var existingButtons = GetComponentsInChildren<BuildingBtn>(true);

        int index = 0;

        foreach (var data in App.GetData<TitleData>().Building.Values)
        {
            if (data.Type != (int)_buildingType)
            {
                continue;
            }

            if (index < existingButtons.Length)
            {
                buildingBtn = existingButtons[index];
            }
            else
            {
                buildingBtn = Instantiate(_buildingBtnPrefab, _buildingBtnParent).GetComponent<BuildingBtn>();
            }

            buildingBtn.Initialize(data, _buildingObjectParent);
            _btnDictionary.Add(data.ID, buildingBtn);

            index++;
        }

        for (; index < existingButtons.Length; index++)
        {
            existingButtons[index].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (CanScrollDown())
        {
            _scrollUpObj.SetActive(false);
            _scrollDownObj.SetActive(true);
        }
        else if (CanScrollUp())
        {
            _scrollUpObj.SetActive(true);
            _scrollDownObj.SetActive(false);
        }
        else
        {
            _scrollUpObj.SetActive(false);
            _scrollDownObj.SetActive(false);
        }
    }

    private bool CanScrollDown()
    {
        float contentHeight = _contentRect.rect.height;
        float viewportHeight = _viewportRect.rect.height;

        return _contentRect.anchoredPosition.y < (contentHeight - viewportHeight);
    }

    private bool CanScrollUp()
    {
        return _contentRect.anchoredPosition.y > 0.01f;
    }
}