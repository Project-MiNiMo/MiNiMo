using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum ResourceType
{
    Resource,
    SpecialResource
}

public class GetItemPanel : UIBase
{
    protected override GameObject Panel => _itemBack;
    
    [SerializeField] private GameObject _itemBack;
    [SerializeField] private Image[] _itemImgs;
    [SerializeField] private Image[] _iconImgs;
    [SerializeField] private Button _closeBtn;
    
    private ItemSO _itemSO;

    public override void Initialize()
    {
        _itemSO = App.GetData<TitleData>().ItemSO;
        _closeBtn.onClick.AddListener(ClosePanel);
    }
    
    public override void OpenPanel()
    {
        base.OpenPanel();
        
        SetItemsNull();
    }

    public void OpenPanel(List<Item> items)
    {
        OpenPanel();
        SetItems(items);
    }


    public void OpenPanel(Item item)
    {
        OpenPanel();
        SetItem(item);
    }

    public void OpenPanel(string code, int count)
    {
        var item = _itemSO.GetItem(code);
        OpenPanel(item);
    }
    
    private void SetItems(List<Item> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            SetItem(items[i], i);
        }
    }

    private void SetItem(Item item, int index = 0)
    {
        _itemImgs[index].gameObject.SetActive(true);
        _itemImgs[index].sprite = item.Icon;
            
        _iconImgs[index].gameObject.SetActive(true);
        _iconImgs[index].sprite = item.Icon;
    }

    private void SetItemsNull()
    {
        for (var i = 0; i < _itemImgs.Length; i++) 
        {
            _itemImgs[i].gameObject.SetActive(false);
            _iconImgs[i].gameObject.SetActive(false);
        }
    }
    
    private IEnumerator ShowResources(List<Item> items)
    {
        yield return null;
    }
}
