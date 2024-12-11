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

    private Item _item;
    private int _currentCount;

    public void Initialize(Item item)
    {
        _item = item;
        _currentCount = (item.Count / 2) + 1;
        _countText.text = _currentCount.ToString();
        _priceText.text = item.Data.ToString();

        _increaseBtn.onClick.AddListener(() => AddCurrentCount(1));
        _decreaseBtn.onClick.AddListener(() => AddCurrentCount(-1));
        //_sellBtn.onClick.AddListener(OnClickSellBtn);
    }

    private void AddCurrentCount(int amount)
    {
        _currentCount += amount;
        _countText.text = _currentCount.ToString();
    }
    
    
}
