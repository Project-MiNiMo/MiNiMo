using System;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UseDiamondType
{
    Produce,
    ProduceExpand,
}

public class DiamondPanel : UIBase
{
    [SerializeField] private GameObject _useBack;
    [SerializeField] private TextMeshProUGUI _useTitleTMP;
    [SerializeField] private TextMeshProUGUI _useDescriptionTMP;
    [SerializeField] private TextMeshProUGUI _diamondCountTMP;
    [SerializeField] private Button _useBtn;
    [SerializeField] private Button _useCloseBtn;
    
    [SerializeField] private GameObject _chargeBack;
    [SerializeField] private TextMeshProUGUI _chargeTitleTMP;
    [SerializeField] private TextMeshProUGUI _chargeDescriptionTMP;
    [SerializeField] private Button _chargeYesBtn;
    [SerializeField] private Button _chargeNoBtn;
    [SerializeField] private Button _chargeCloseBtn;
    
    private PlayerData _playerData;
    private TitleData _titleData;
    private string _diamondCountString;
    
    private Action _useAction;
    private int _useCount;
    
    public override void Initialize()
    {
        _playerData = App.GetData<PlayerData>();
        _titleData = App.GetData<TitleData>();

        SetString();
        SetEvent();
        
        ClosePanel();
    }

    private void SetString()
    {
        _useTitleTMP.text = _titleData.GetString("STR_SPENDCASH_TITLE");
        _diamondCountString = _titleData.GetString("STR_SPENDCASH_BUTTON");
        
        _chargeTitleTMP.text = _titleData.GetString("STR_CHARGECASH_TITLE");
        _chargeDescriptionTMP.text = _titleData.GetString("STR_CHARGECASH_DESC");
        
        _chargeYesBtn.GetComponentInChildren<TextMeshProUGUI>().text = _titleData.GetString("STR_BUTTON_YES");
        _chargeNoBtn.GetComponentInChildren<TextMeshProUGUI>().text = _titleData.GetString("STR_BUTTON_NO");
    }

    private void SetEvent()
    {
        _useBtn.onClick.AddListener(OnClickUse);
        
        _chargeYesBtn.onClick.AddListener(OnClickChargeYes);
        _chargeNoBtn.onClick.AddListener(OnClickChargeNo);
        
        _useCloseBtn.onClick.AddListener(ClosePanel);
        _chargeCloseBtn.onClick.AddListener(ClosePanel);
    }
    
    public void OpenPanel(UseDiamondType type, int count, Action useAction)
    {
        base.OpenPanel();
        
        _useBack.SetActive(true);
        _chargeBack.SetActive(false);
        
        _useDescriptionTMP.text = _titleData.GetString(GetUseDescription(type));
        _diamondCountTMP.text = string.Format(_diamondCountString, count);
        
        _useAction = useAction;
        _useCount = count;
    }
    
    private string GetUseDescription(UseDiamondType type) => type switch
    {
        UseDiamondType.Produce => "STR_SPENDCASH_DESC_PRODUCE",
        _ => string.Empty
    };
    
    private void OnClickUse()
    {
        if (_playerData.DiamondStar < _useCount)
        {
            _useBack.SetActive(false);
            _chargeBack.SetActive(true);
        }
        else
        {
            _playerData.DiamondStar -= _useCount;
            _useAction?.Invoke();
            _useAction = null;
            _useCount = 0;

            ClosePanel();
        }
    }
    
    private void OnClickChargeYes()
    {
        //TODO : Charge Panel Open
        ClosePanel();
    }
    
    private void OnClickChargeNo()
    {
        ClosePanel();
    }
}
