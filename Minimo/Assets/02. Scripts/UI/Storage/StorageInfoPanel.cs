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

        _sellCtrl.Setup();
        
        ClosePanel();
    }

    public void OpenPanel(StorageBtn storageBtn)
    {
        base.OpenPanel();
        
        SetInfo(storageBtn.Item);
        _sellCtrl.Initialize(storageBtn.Item);
        
        Debug.Log(storageBtn.SibilingsIndex);
        Debug.Log(storageBtn.Position.y);
        
        var newPosition = new Vector2(GetPositionBySiblingIndex(storageBtn.SibilingsIndex), 0);
        _infoRect.anchoredPosition = newPosition;

        newPosition = new Vector2(_infoRect.position.x, storageBtn.Position.y - 100);
        _infoRect.position = newPosition;
    }

    private void SetInfo(Item item)
    {
        _nameTMP.text = _titleData.GetString(item.Data.Name);
        _descriptionTMP.text = _titleData.GetString(item.Data.Description);
        _iconImg.sprite = item.Icon;
    }

    private int GetPositionBySiblingIndex(int index) => index switch
    {
        0 => -50,
        1 => 250,
        2 => -250,
        3 => 50,
        _ => 0
    };
}
