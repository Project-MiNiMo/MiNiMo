using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class StorageBack : MonoBehaviour
{
    private enum StorageType
    {
        Entire,
        Ingredient,
        Product,
        Construction
    }

    [SerializeField] private Button[] _btns;

    [SerializeField] private Transform _storageBtnParent;
    [SerializeField] private GameObject _storageBtnPrefab;

    [Header("Scroll")]
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] private GameObject _scrollUpObj;
    [SerializeField] private GameObject _scrollDownObj;

    private RectTransform _contentRect;
    private RectTransform _viewportRect;

    private List<StorageBtn> _storageBtns;
    
    private void Start()
    {
        _contentRect = _scrollRect.content;
        _viewportRect = _scrollRect.viewport;

        SetButtonEvent();
        InitStorageBtns();
    }

    private void OnEnable()
    {
        if (_scrollRect == null) return;

        _scrollRect.verticalNormalizedPosition = 1;
    }
    
    private void SetButtonEvent()
    {
        for (int i = 0; i < _btns.Length; i++)
        {
            int idx = i;

            _btns[idx].onClick.AddListener(() => OnClickStorageBtn(idx));
        }
    }

    private void InitStorageBtns()
    {
        StorageBtn storageBtn;

        var existingButtons = GetComponentsInChildren<StorageBtn>(true);

        var items = App.GetData<TitleData>().ItemSO.items;
        _storageBtns = new(items.Length);

        int index = 0;

        foreach (var item in items)
        {
            if (index < existingButtons.Length)
            {
                storageBtn = existingButtons[index];
            }
            else
            {
                storageBtn = Instantiate(_storageBtnPrefab, _storageBtnParent).GetComponent<StorageBtn>();
            }

            storageBtn.Initialize(item);

            index++;
        }

        for (; index < existingButtons.Length; index++)
        {
            existingButtons[index].gameObject.SetActive(false);
        }
    }
    
    private void OnClickStorageBtn(int index)
    {
        if ((StorageType)index == StorageType.Entire) 
        {
            foreach (var button in _storageBtns)
            {
                button.gameObject.SetActive(true);
            }

            _scrollRect.verticalNormalizedPosition = 1;
            return;
        }
        
        foreach (var button in _storageBtns)
        {
            if (CheckType((ItemType)button.Item.Data.Type) == (StorageType)index) 
            {
                button.gameObject.SetActive(true);
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
        
        _scrollRect.verticalNormalizedPosition = 1;
    }

    private StorageType CheckType(ItemType type) => type switch
    {
        ItemType.Seed => StorageType.Ingredient,
        ItemType.Harvest => StorageType.Ingredient,
        ItemType.Ingredient => StorageType.Ingredient,
        ItemType.Product => StorageType.Product,
        ItemType.Construction => StorageType.Construction,
        _ => StorageType.Entire
    };

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
