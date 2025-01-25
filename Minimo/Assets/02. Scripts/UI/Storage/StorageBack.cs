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
    
    [SerializeField] private Transform _storageBtnParent;
    [SerializeField] private GameObject _storageBtnPrefab;

    [Header("Scroll")]
    [SerializeField] private ScrollRect _scrollRect;
    
    public List<StorageBtn> _storageBtns;

    public void InitStorageBtns()
    {
        var existingButtons = GetComponentsInChildren<StorageBtn>(true);
        var items = App.GetData<TitleData>().ItemSO.items;
        
        _storageBtns = new(items.Count);

        int i = 0;
        
        for (; i < items.Count; i++)
        {
            var storageBtn = i < existingButtons.Length 
                ? existingButtons[i] 
                : Instantiate(_storageBtnPrefab, _storageBtnParent).GetComponent<StorageBtn>();

            storageBtn.Initialize(items[i]);
            _storageBtns.Add(storageBtn);
        }

        for (; i < existingButtons.Length; i++)
        {
            existingButtons[i].gameObject.SetActive(false);
        }
    }
    
    public void FilterStorageBtns(int index)
    {
        var targetType = (StorageType)index;
        
        foreach (var button in _storageBtns)
        {
            bool isActive = 
                targetType == StorageType.Entire 
                || CheckType((ItemType)button.Item.Data.Type) == targetType;
            
            button.gameObject.SetActive(isActive);
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
