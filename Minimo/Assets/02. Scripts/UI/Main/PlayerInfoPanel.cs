using UnityEngine;
using TMPro;

public class PlayerInfoPanel : UIBase
{
    [SerializeField] private TextMeshProUGUI _nicknameTMP;
    [SerializeField] private TextMeshProUGUI _levelTMP;

    private AccountInfoManager _accountInfo;
    private string _levelString;
    
    public override void Initialize()
    {
        var titleData = App.GetData<TitleData>();
        _accountInfo = App.GetManager<AccountInfoManager>();
        
        _levelString = titleData.GetString("STR_PROFILE_LEVEL");
        
        _nicknameTMP.text = _accountInfo.NickName;
        _levelTMP.text = string.Format(_levelString, _accountInfo.Level);
    }
    
    public void SetNickName()
    {
        _nicknameTMP.text = _accountInfo.NickName;
    }
}
