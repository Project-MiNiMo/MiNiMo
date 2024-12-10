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

    private int _currentCount;

    private void Start()
    {
        
    }

    public void Initialize(Item item)
    {
        _countText.text = 1.ToString();
        _priceText.text = item.Data.ToString();
        
        //_increaseBtn.onClick.AddListener(OnClickIncreaseBtn);
        //_decreaseBtn.onClick.AddListener(OnClickDecreaseBtn);
        //_sellBtn.onClick.AddListener(OnClickSellBtn);
    }
    
    
}
