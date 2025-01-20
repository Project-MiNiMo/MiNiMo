using System;
using MinimoShared;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UseCashBack : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private TextMeshProUGUI _descriptionTMP;
    [SerializeField] private TextMeshProUGUI _priceTMP;
    [SerializeField] private Button _useBtn;
    [SerializeField] private Button _closeBtn;
    
    private AccountInfoManager _accountInfo;
    private TitleData _titleData;
    
    private Action _closeAction;
    private Action _useAction;
    private Action _lackAction;
    private int _price;
    
    private string _priceString;
    
    public void Initialize(AccountInfoManager accountInfo, TitleData titleData, Action closeAction, Action lackAction)
    {
        _accountInfo = accountInfo;
        _titleData = titleData;
        _closeAction = closeAction;
        _lackAction = lackAction;

        _titleTMP.text = _titleData.GetString("STR_SPENDCASH_TITLE");
        _priceString = _titleData.GetString("STR_SPENDCASH_BUTTON");

        _useBtn.onClick.AddListener(OnClickUse);
        _closeBtn.onClick.AddListener(() => _closeAction?.Invoke());
    }
    
    public void Setup(UseCashType type, int price, Action useAction)
    {
        _descriptionTMP.text = _titleData.GetString(GetUseDescription(type));
        _priceTMP.text = string.Format(_priceString, price);

        _useAction = useAction;
        _price = price;
    }

    private string GetUseDescription(UseCashType type) => type switch
    {
        UseCashType.Produce => "STR_SPENDCASH_DESC_PRODUCE",
        UseCashType.ProduceExpand => "STR_SPENDCASH_DESC_PRODUCE",
        UseCashType.ProduceMaterial => "STR_SPENDCASH_DESC_PRODUCE",
        _ => string.Empty
    };

    private void OnClickUse()
    {
        if (_accountInfo.BlueStar < _price)
        {
            _lackAction?.Invoke();
        }
        else
        {
            var newCurrencyRequest = new CurrencyDTO
            {
                Star = _accountInfo.Star,
                BlueStar = _accountInfo.BlueStar - _price
            };
            
            _accountInfo.UpdateCurrency(newCurrencyRequest);
            _useAction?.Invoke();
            _useAction = null;
            _price = 0;

            _closeAction.Invoke();
        }
    }
}
