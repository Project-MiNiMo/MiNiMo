using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageSellCtrl : MonoBehaviour
{
    [SerializeField] private Button _increaseBtn;
    [SerializeField] private Button _decreaseBtn;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private Button _sellBtn;
    [SerializeField] private TextMeshProUGUI _priceText;

    private PlayerData _playerData;
    private StoragePanel _storagePanel;
    private StorageInfoPanel _infoPanel;
    
    private Item _item;
    
    private int _currentCount;
    private string _sellText;

    private void Start()
    {
        _playerData = App.GetData<PlayerData>();
        _storagePanel = App.GetManager<UIManager>().GetPanel<StoragePanel>();
        _infoPanel = App.GetManager<UIManager>().GetPanel<StorageInfoPanel>();
        
        _sellText = App.GetData<TitleData>().GetString("STR_STORAGE_UI_SELL");
        
        _increaseBtn.onClick.AddListener(() => AddCurrentCount(1));
        _decreaseBtn.onClick.AddListener(() => AddCurrentCount(-1));
        _sellBtn.onClick.AddListener(OnClickSell);
    }
    
    public void Initialize(Item item)
    {
        _item = item;
        
        _currentCount = (item.Count / 2) + 1;
        UpdateCurrentCount();
    }
    
    private void UpdateCurrentCount()
    {
        _countText.text = $"X{_currentCount}";
        _priceText.text = string.Format(_sellText, Mathf.Max(_item.Data.SellCost * _currentCount, 0));
        
        UpdateButtonActive();
    }
    
    private void AddCurrentCount(int amount)
    {
        _currentCount += amount;
        
        UpdateCurrentCount();
    }
    
    private void UpdateButtonActive()
    {
        if (_currentCount <= 1) 
        {
            _decreaseBtn.gameObject.SetActive(false);
        }
        else
        {
            _decreaseBtn.gameObject.SetActive(true);
        }

        if (_currentCount >= _item.Count) 
        {
            _increaseBtn.gameObject.SetActive(false);
        }
        else
        {
            _increaseBtn.gameObject.SetActive(true);
        }
    }

    private void OnClickSell()
    {
        _item.Count -= _currentCount;
        _playerData.BlueStar += _currentCount * _item.Data.SellCost;
        
        _storagePanel.OnStorageChanged();
        _infoPanel.ClosePanel();
    }
}
