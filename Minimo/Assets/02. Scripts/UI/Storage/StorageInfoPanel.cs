using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageInfoPanel : UIBase
{
    [SerializeField] private RectTransform _infoRect;
    
    [SerializeField] private Button _closeBtn;
    
    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _descriptionTMP;
    [SerializeField] private Image _iconImg;
    [SerializeField] private StorageSellCtrl _sellCtrl;
    
    private TitleData _titleData;
    
    public override void Initialize()
    {
        _titleData = App.GetData<TitleData>();
        _closeBtn.onClick.AddListener(ClosePanel);
    }

    public void OpenPanel(StorageBtn storageBtn)
    {
        base.OpenPanel();
        
        SetInfo(storageBtn.Item);
        _sellCtrl.Initialize(storageBtn.Item);
        _infoRect.anchoredPosition = storageBtn.AnchoredPosition;
    }

    private void SetInfo(Item item)
    {
        _nameTMP.text = _titleData.GetString(item.Data.Name);
        _descriptionTMP.text = _titleData.GetString(item.Data.Description);
        _iconImg.sprite = item.Icon;
    }
}
