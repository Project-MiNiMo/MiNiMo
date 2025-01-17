using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StorageExpandPanel : UIBase
{
    [SerializeField] private StorageCapacityCtrl _storageCapacityCtrl;
    [SerializeField] private Button _closeBtn;

    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private TextMeshProUGUI _descriptionTMP;

    [SerializeField] private Button _increaseBtn;
    [SerializeField] private Button _decreaseBtn;
    [SerializeField] private TextMeshProUGUI _countTMP;
    [SerializeField] private TextMeshProUGUI _capacityTMP;
    
    [SerializeField] private Button _expandBtn;
    [SerializeField] private TextMeshProUGUI _priceTMP;
    
    private PlayerData _playerData;
    private int _currentCount;
    private int _expandCost;
    
    private string _expandString;
    
    public override void Initialize()
    {
        var titleData = App.GetData<TitleData>();
        
        _playerData = App.GetData<PlayerData>();
        
        _expandCost = titleData.Common["WareHouseExpandCost"];
        _expandString = titleData.GetString("STR_STORAGE_EXPAND_COST");
        
        _titleTMP.text = titleData.GetString("STR_STORAGE_EXPAND_TITLE");
        _descriptionTMP.text = titleData.GetString("STR_STORAGE_EXPAND_DESC");
        
        _closeBtn.onClick.AddListener(ClosePanel);
        
        _increaseBtn.onClick.AddListener(() => AddCurrentCount(10));
        _decreaseBtn.onClick.AddListener(() => AddCurrentCount(-10));
        
        _expandBtn.onClick.AddListener(OnClickExpand);
        
        ClosePanel();
    }

    public override void OpenPanel()
    {
        base.OpenPanel();
        
        _currentCount = 10;
        _capacityTMP.text = $"{_storageCapacityCtrl.Capacity} / {_playerData.MaxStorageCapacity}";

        UpdateCurrentCount();
    }

    private void OnClickExpand()
    {
        if (_playerData.BlueStar < _expandCost * _currentCount) return;
        
        _playerData.BlueStar -= _expandCost * _currentCount;
        _playerData.MaxStorageCapacity += _currentCount;
        
        _storageCapacityCtrl.UpdateMaxCapacity();
        
        ClosePanel();
    }
    
    private void AddCurrentCount(int amount)
    {
        _currentCount += amount;
        
        UpdateCurrentCount();
    }
    
    private void UpdateCurrentCount()
    {
        _countTMP.text = (_currentCount + _playerData.MaxStorageCapacity).ToString();
        _priceTMP.text = string.Format(_expandString, Mathf.Max(_expandCost * _currentCount, 0));
        
        UpdateButtonActive();
    }
    
    private void UpdateButtonActive()
    {
        if (_currentCount <= 10) 
        {
            _decreaseBtn.gameObject.SetActive(false);
        }
        else
        {
            _decreaseBtn.gameObject.SetActive(true);
        }

        if (_currentCount >= 200) 
        {
            _increaseBtn.gameObject.SetActive(false);
        }
        else
        {
            _increaseBtn.gameObject.SetActive(true);
        }
    }
}
