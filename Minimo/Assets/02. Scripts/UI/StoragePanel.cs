using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoragePanel : UIBase
{
    [SerializeField] private Transform _storageGroup;
    [SerializeField] private GameObject _storageBtnPrefab;
    [SerializeField] private Transform _buildingGroup;

    [Header("Buttons")]
    [SerializeField] private Button _openBtn;
    [SerializeField] private Button _closeBtn;

    private RectTransform _rect;

    private Dictionary<string, StorageBtn> _btnDictionary = new();

    private bool _isOpen = true;

    public override void Initialize()
    {
        _rect = GetComponent<RectTransform>();

        _openBtn.onClick.AddListener(Open);
        _closeBtn.onClick.AddListener(Close);

        InitStorageButtons();
        UpdateOpenCloseButtons();
    }

    private void InitStorageButtons()
    {
        var existingButtons = _storageGroup.GetComponentsInChildren<StorageBtn>(true);

        int index = 0;

        foreach (var data in App.GetData<TitleData>().Building.Values)
        {
            StorageBtn storageBtn;

            if (index < existingButtons.Length)
            {
                storageBtn = existingButtons[index];
            }
            else
            {
                storageBtn = Instantiate(_storageBtnPrefab, _storageGroup).GetComponent<StorageBtn>();
            }

            storageBtn.Initialize(data, _buildingGroup);
            _btnDictionary.Add(data.ID, storageBtn);

            index++;
        }
    }

    private void UpdateOpenCloseButtons()
    {
        _openBtn.gameObject.SetActive(!_isOpen);
        _closeBtn.gameObject.SetActive(_isOpen);
    }

    public void AddObjectCount(string objectName, int count)
    {
        string cleanName = objectName.Replace("(Clone)", string.Empty);

        if (_btnDictionary.TryGetValue(cleanName, out var storageBtn))
        {
            storageBtn.AddObjectCount(count);
        }
        else
        {
            Debug.LogWarning($"Storage button for '{cleanName}' not found.");
        }
    }

    public void Open()
    {
        if (_isOpen)
        {
            return;
        }

        _rect.DOAnchorPosY(0f, 0.5f).OnComplete(() =>
        {
            _isOpen = true;
            UpdateOpenCloseButtons();
        });
    }

    public void Close()
    {
        if (!_isOpen)
        {
            return;
        }

        _rect.DOAnchorPosY(-_rect.sizeDelta.y, 0.2f).OnComplete(() =>
        {
            _isOpen = false;
            UpdateOpenCloseButtons();
        });
    }
}
