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

    private RectTransform _contentRect;
    private RectTransform _viewportRect;

    private List<StorageBtn> _storageBtns;
    
    private void Start()
    {
        SetButtonEvent();
        InitStorageBtns();
    }

    private void OnEnable()
    {
        OnClickStorageBtn(0);
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
            _storageBtns.Add(storageBtn);

            index++;
        }

        for (; index < existingButtons.Length; index++)
        {
            existingButtons[index].gameObject.SetActive(false);
        }
    }
    
    private void OnClickStorageBtn(int index)
    {
        if (_storageBtns == null) return;
        
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
                Debug.Log($"{CheckType((ItemType)button.Item.Data.Type)} : {(StorageType)index}");
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
}
