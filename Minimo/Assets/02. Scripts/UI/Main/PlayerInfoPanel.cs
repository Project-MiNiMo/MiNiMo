using UniRx;
using UnityEngine;
using TMPro;

public class PlayerInfoPanel : UIBase
{
    [SerializeField] private TextMeshProUGUI _nicknameTMP;
    [SerializeField] private TextMeshProUGUI _levelTMP;
    [SerializeField] private TextMeshProUGUI _blueStarTMP;
    [SerializeField] private TextMeshProUGUI _rainbowStarTMP;

    private AccountInfoManager _accountInfo;
    
    public override void Initialize()
    {
        _accountInfo = App.GetManager<AccountInfoManager>();
  
        _nicknameTMP.text = _accountInfo.NickName.Value;
        _levelTMP.text = _accountInfo.Level.Value.ToString();
        
        _blueStarTMP.text = _accountInfo.Star.ToString();
        _rainbowStarTMP.text = _accountInfo.BlueStar.ToString();
        
        _accountInfo.NickName
            .Subscribe((nickname) => _nicknameTMP.text = nickname)
            .AddTo(gameObject);

        _accountInfo.Level
            .Subscribe((level) => _levelTMP.text = level.ToString());

        _accountInfo.Star
            .Subscribe((star) => _blueStarTMP.text = star.ToString("N0"));
        
        _accountInfo.BlueStar
            .Subscribe((blueStar)=> _rainbowStarTMP.text = blueStar.ToString("N0"));
    }
}
