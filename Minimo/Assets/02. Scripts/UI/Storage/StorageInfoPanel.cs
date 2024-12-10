using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageInfoPanel : UIBase
{
    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _descriptionTMP;
    [SerializeField] private Image _iconImg;
    [SerializeField] private StorageSellCtrl _sellBtn;
    
    private TitleData _titleData;
    
    public override void Initialize()
    {
        _titleData = App.GetData<TitleData>();
    }

    public void OpenPanel(Item item)
    {
        base.OpenPanel();
        
        SetInfo(item);
        _sellBtn.Initialize(item);
    }

    private void SetInfo(Item item)
    {
        _nameTMP.text = _titleData.GetString(item.Data.Name);
        _descriptionTMP.text = _titleData.GetString(item.Data.Description);
        _iconImg.sprite = Resources.Load<Sprite>($"Item/Icon/{item.Icon}");
    }
}
