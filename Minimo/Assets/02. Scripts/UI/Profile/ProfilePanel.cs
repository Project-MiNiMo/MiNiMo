using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfilePanel : UIBase
{
    protected override GameObject Panel => _profileBack;
    
    [SerializeField] private Button _openBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private GameObject _profileBack;

    [SerializeField] private TMP_InputField _nickNameInput;
    [SerializeField] private Image _playerImg;
    [SerializeField] private TextMeshProUGUI _levelTMP;

    [SerializeField] private TextMeshProUGUI _expTMP;
    [SerializeField] private Image _expProgressBarImg;

    [SerializeField] private Image _constellationImg;

    [SerializeField] private TextMeshProUGUI _followerTMP;
    [SerializeField] private TextMeshProUGUI _followerDescTMP;
    [SerializeField] private TextMeshProUGUI _visitorTMP;
    [SerializeField] private TextMeshProUGUI _visitorDescTMP;
    [SerializeField] private TextMeshProUGUI _friendTMP;
    [SerializeField] private TextMeshProUGUI _friendDescTMP;

    private AccountInfoManager _accountInfo;
    private PlayerInfoPanel _playerInfoPanel;
    
    private string _levelString;
    private string _expString;
    
    #region Temp - PlayerInfo
    private const int PLAYER_LEVEL = 5;
    private const int PLAYER_EXP = 33;
    private const int PLAYER_EXP_MAX = 100;
    private const int PLAYER_FOLLOWER = 5;
    private const int PLAYER_VISITOR = 10;
    private const int PLAYER_FRIEND = 15;
    #endregion
    
    public override void Initialize()
    {
        _accountInfo = App.GetManager<AccountInfoManager>();
        _playerInfoPanel = App.GetManager<UIManager>().GetPanel<PlayerInfoPanel>();
        
        _openBtn.onClick.AddListener(OpenPanel);
        _closeBtn.onClick.AddListener(ClosePanel);
        _nickNameInput.onValueChanged.AddListener((text)=>
        {
            _accountInfo.UpdateNickname(text);
        });
        
        _accountInfo.NickName.Subscribe((nickName) =>
        {
            _nickNameInput.SetTextWithoutNotify(nickName);
        }).AddTo(gameObject);

        SetString();
        SetButtonEvent();

        UpdateExpProgressBar();
    }

    #region Initialize
    private void SetString()
    {
        var titleData = App.GetData<TitleData>();

        _nickNameInput.SetTextWithoutNotify(_accountInfo.NickName.Value);
            
        _levelString = titleData.GetString("STR_PROFILE_LEVEL");
        _expString = titleData.GetString("STR_PROFILE_EXP");

        _levelTMP.text = string.Format(_levelString, _accountInfo.Level);
        _expTMP.text = titleData.GetFormatString("STR_PROFILE_EXP", PLAYER_EXP.ToString(), PLAYER_EXP_MAX.ToString());

        _followerDescTMP.text = titleData.GetString("STR_PROFILE_FOLLOWER");
        _visitorDescTMP.text = titleData.GetString("STR_PROFILE_VISITOR");
        _friendDescTMP.text = titleData.GetString("STR_PROFILE_FRIEND");

        _followerTMP.text = PLAYER_FOLLOWER.ToString();
        _visitorTMP.text = PLAYER_VISITOR.ToString();
        _friendTMP.text = PLAYER_FRIEND.ToString();
    }

    private void SetButtonEvent()
    {
        _openBtn.onClick.AddListener(OpenPanel);
        _closeBtn.onClick.AddListener(ClosePanel);
    }
    #endregion

    private void UpdateExpProgressBar()
    {
        _expProgressBarImg.fillAmount = (float)PLAYER_EXP / PLAYER_EXP_MAX;
    }
}
